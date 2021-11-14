using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ZwajApp.API.Data;
using ZwajApp.API.DTOs;
using ZwajApp.API.Helpers;
using ZwajApp.API.Models;

namespace ZwajApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IZwajRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryCofig;
        private Cloudinary _cloudinary;

        public PhotosController(IZwajRepository repo, IOptions<CloudinarySettings> cloudinaryCofig,
        
        IMapper mapper)
        {
            _mapper = mapper;
            _cloudinaryCofig = cloudinaryCofig;
            _repo = repo;
            Account acc =new Account(
                _cloudinaryCofig.Value.CloudName,
                _cloudinaryCofig.Value.ApiKey,
                _cloudinaryCofig.Value.ApiSecret

            );
            _cloudinary= new Cloudinary(acc);
        }
        [HttpGet("{id}", Name = "GetPhoto") ]
        public async Task<IActionResult> GetPhoto(int id){
            var photoFromRepository = await _repo.GetPhoto(id);
            var photo= _mapper.Map<PhotoForReturnDto>(photoFromRepository);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm]PhotoForCreateDto photoForCreateDto){
             if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var userFromRepo= await _repo.GetUser(userId);
            var file=photoForCreateDto.File;
            var uploadResult= new ImageUploadResult();
            if(file !=null && file.Length>0){
                using(var stream = file.OpenReadStream()){
                    var uploadPramas= new ImageUploadParams(){
                        File=new FileDescription(file.Name,stream),
                        Transformation= new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult=_cloudinary.Upload(uploadPramas);
                }
            }
            photoForCreateDto.Url=uploadResult.Uri.ToString();
            photoForCreateDto.publicId=uploadResult.PublicId;
            var photo= _mapper.Map<Photo>(photoForCreateDto);
            if(!userFromRepo.Photos.Any(c=>c.IsMain))photo.IsMain=true;
            userFromRepo.Photos.Add(photo);
            if(await _repo.SaveAll()){
                var photoToReturn=_mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new{id = photo.Id},photoToReturn);
            }
            
            return BadRequest("خطأ فى أضافة الصورة");
        }
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId,int id){
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var userFromRepo= await _repo.GetUser(userId);
            if(!userFromRepo.Photos.Any(c=>c.Id==id))return Unauthorized();
            var desiredMainPhoto= await _repo.GetPhoto(id);
            if(desiredMainPhoto.IsMain)return BadRequest("هذه هى الصورة الاساسية بالفعل");
            var currentMainPhoto = await _repo.GetMainPhotoForUser(userId);
            currentMainPhoto.IsMain=false;
            desiredMainPhoto.IsMain=true;
            if(await _repo.SaveAll())return NoContent();
            return BadRequest("لا يمكن تعديل الصورة الاساسية");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id){
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
            var userFromRepo= await _repo.GetUser(userId);
            if(!userFromRepo.Photos.Any(c=>c.Id==id))return Unauthorized();
            var photo= await _repo.GetPhoto(id);
            if(photo.IsMain)return BadRequest("لا يمكن حذف الصورة الاساسية");
            
            if(photo.PublicId!=null){
                var deleteParams = new DeletionParams(photo.PublicId);
                var result =this._cloudinary.Destroy(deleteParams);
                if(result.Result=="ok"){
                    _repo.Delete(photo);
                }
            }

             if(photo.PublicId == null){
                    _repo.Delete(photo);
                }

                if(await _repo.SaveAll())
                return Ok();
                return BadRequest("فشل فى حذف الصورة");
        }
    }
}