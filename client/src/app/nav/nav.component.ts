import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {}
  
<<<<<<< HEAD

  constructor(public accountService: AccountService,private router: Router) { }

=======
  constructor(public accountService: AccountService,private router: Router) { }
>>>>>>> 208cfe55ccef7a93f0d7df461fd1583bd02af5bf

  ngOnInit(): void {
    
  }

  login()
  {
    this.accountService.login(this.model).subscribe(Response => {
      this.router.navigateByUrl('/members');
      
    }, error =>{
      console.log(error);
    })
  }

  logout()
  {
    this.accountService.logout();
    this.router.navigateByUrl('/');
<<<<<<< HEAD

=======
   
>>>>>>> 208cfe55ccef7a93f0d7df461fd1583bd02af5bf
  }
}
