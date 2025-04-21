import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MasterService } from '../../services/master.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  register: any = { 
    "userId": 0,
    "userName": "",
    "emailId": "",
    "fullName": "",
    "role": "",
    "createdDate": new Date(),
    "password": "" 
  }

  constructor(
    private masterService: MasterService,
    private router: Router
  ) {}

  createUser() {
    this.masterService.createUser(this.register).subscribe((res:any) => {
      if(res.result) {
        alert('User Created Successfully');
        this.router.navigate(['/login']); // Redirect to login after registration
      } else {
        alert(res.message);
      }
    });
  }
}