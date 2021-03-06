import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { User } from '../_models/user';
import { stringify } from 'querystring';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  jwtHelper = new JwtHelperService();
  baseUrl = environment.apiUrl+'auth/';
  decodedToken:any;
  currentUser:User;
  photoUrl = new BehaviorSubject<string>('..//..//assets/User.png')
  currentPhotoUrl= this.photoUrl.asObservable();

  constructor(private http: HttpClient) { }

  changeMemberPhoto(newPhotoUrl:string){
    this.photoUrl.next(newPhotoUrl);
  }

  login(user: User) {
    return this.http.post(this.baseUrl + 'login', user).pipe(
      map((response: any) => {
        const user = response;
        if (user) { localStorage.setItem('token', user.token);
        localStorage.setItem('user',JSON.stringify(user.user))
      this.decodedToken=this.jwtHelper.decodeToken(user.token);
      this.currentUser=user.user;
      this.changeMemberPhoto(this.currentUser.photoUrl);
      }
      }))
  }

  register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
  }

  loggedIn() {
    
    try{const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);}
    catch{
      return false
    }
      
    


  }


}
