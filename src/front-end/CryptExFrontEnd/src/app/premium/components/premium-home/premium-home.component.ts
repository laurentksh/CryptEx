import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/services/auth.service';
import { UserService } from 'src/app/user/services/user.service';

@Component({
  selector: 'app-premium-home',
  templateUrl: './premium-home.component.html',
  styleUrls: ['./premium-home.component.scss']
})
export class PremiumHomeComponent implements OnInit {

  constructor(private authService: AuthService, public userService: UserService) { }

  ngOnInit(): void {
  }

  isPremium(): boolean {
    return this.authService.HasClaim("premium");
  }
}
