import { Injectable } from "@angular/core";
import { AuthService, SocialUser } from "angularx-social-login";
import { IUser } from "../models/user";
import { ICustomer } from "../models/customer";
import { map, switchMap } from "rxjs/operators";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable } from 'rxjs';
import { UserToken } from '../models/usertoken';
import { environment } from 'src/environments/environment';

@Injectable()
export class UserService {
  user: IUser;
  customers: ICustomer[];

  private customerDataUrl = (googleUserId) =>
    `${environment.baseAPIUrl}/Customer/Query?GoogleUserId=${googleUserId}&Page=1&Size=100`;

  constructor(private authService: AuthService, private http: HttpClient) { }

  getUser() {
    return this.authService.authState.pipe(
      map((data) => ({
        email: data.email,
        photoUrl: data.photoUrl,
      })),
      switchMap((data: any) =>
        this.fetchCustomerData(data.email, data.photoUrl)
      )
    );
  }

  signOut(): void {
    this.authService.signOut();
  }

  fetchCustomerData(googleId, photoUrl) {
    let headers = new HttpHeaders({ Authorization: `Bearer ${sessionStorage.getItem('userToken')}` });
    /*console.log(localStorage.getItem('userToken'))
    headers = headers.set('Authorization', `Bearer ${localStorage.getItem('userToken')}`);*/

    return this.http
      .get<{ items: ICustomer[] }>(this.customerDataUrl(googleId), { headers })
      .pipe(map((items) => ({ ...items.items[0], photoUrl: photoUrl })));
  }

  googleLogin(googleUser: SocialUser): Observable<UserToken> {
    /*let headers = new HttpHeaders();
    console.log(localStorage.getItem('userToken'))
    headers = headers.set('Authorization', `Bearer ${localStorage.getItem('userToken')}`);*/
    return this.http.post<UserToken>(`${environment.baseAPIUrl}/googlelogin`, { idToken: googleUser.idToken });
  }
}
