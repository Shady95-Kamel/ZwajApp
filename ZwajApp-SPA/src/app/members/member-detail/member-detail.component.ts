import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { UserService } from 'src/app/_services/user.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  user: User
  created:string;
  age:string;
  showIntro:boolean=true;
  showLook:boolean=true;
  options = {weekday : 'long' , year :'numeric' , month : 'long',day:'numeric'};

  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService, private alertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    // this.loadUser();
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.galleryOptions=[{
      width:'500px',height:'500px',imagePercent:100,thumbnailsColumns:4,
      imageAnimation:NgxGalleryAnimation.Slide,preview:false
    }]

    this.galleryImages=this.getImages();
    this.created = new Date(this.user.created).toLocaleString('ar-EG',this.options).replace('،','');
    this.age = this.user.age.toLocaleString('ar-EG');
    this.showIntro=true;
    this.showLook=true;

  }
  getImages(){
    const imageUrls=[];
    for(let i =0;i<this.user.photos.length;i++){
      imageUrls.push({
        small:this.user.photos[i].url,
        medium:this.user.photos[i].url,
        big:this.user.photos[i].url,
      })
    };
    return imageUrls;
  }

  // loadUser(){
  //   this.userService.getUser(+this.route.snapshot.params['id']).subscribe(
  //     (user:User)=>{this.user=user},
  //     error=>{this.alertify.error(error)}
  //   )
  // }



}
