import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-homepage',
  templateUrl: './homepage.component.html',
  styleUrls: ['./homepage.component.css']
})
export class HomepageComponent implements OnInit {
  registerMode=false;
  


  constructor() { }

  ngOnInit(): void {
  }


  registerToggle(){
    this.registerMode=!this.registerMode;
  }
 
  cancelRegisterMode(event: boolean){
  this.registerMode = event;
  }

}
