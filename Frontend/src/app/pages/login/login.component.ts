import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MasterService } from '../../services/master.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginObj: any = {
    "userName": "",
    "password": ""
  }

  constructor(
    private masterService: MasterService,
    private router: Router
  ) {}

  onLogin() {
    if (this.loginObj.userName === 'admin' && this.loginObj.password === '1234') {
      const hardcodedUser = {
        userId: 1,
        userName: 'admin',
        fullName: 'Admin User',
        emailId: 'admin@example.com',
        role: 'Admin',
        createdDate: new Date()
      };
      sessionStorage.setItem('budgetUser', JSON.stringify(hardcodedUser));
      this.router.navigate(['/home']); // Updated navigation
    } else {
      alert('Invalid username or password');
    }
  }
}