import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/_models/user';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { AuthService } from 'src/app/_services/auth.service';
import { UserService } from 'src/app/_services/user.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {

  @ViewChild('editForm') editForm:NgForm

  user:User
  created:string;
  age:string;
  options = {weekday : 'long' , year :'numeric' , month : 'long',day:'numeric'};
  photoUrl:string;

  @HostListener('window:beforeunload',['$event'])

  unLoadNotification($event:any){
    if(this.editForm.dirty){
      $event.returnValue=true;
    }
  }
  constructor(private route:ActivatedRoute , private alertify:AlertifyService , 
    private userService:UserService, private authService:AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data=>{
      this.user=data['user'];
    })
    this.authService.currentPhotoUrl.subscribe(photoUrl=>this.photoUrl=photoUrl);
    this.authService.currentPhotoUrl.subscribe(photoUrl=>this.photoUrl=photoUrl);
    this.created = new Date(this.user.created).toLocaleString('ar-EG',this.options).replace('،','');
    this.age = this.user.age.toLocaleString('ar-EG');
  }
  updateUser(){
    this.userService.updateUser(this.authService.decodedToken.nameid,this.user).subscribe(()=>{
      this.editForm.reset(this.user);
    this.alertify.success('تم تعديل الملف الشخصى بنجاح')
    },error=>this.alertify.error(error))
  }
}
