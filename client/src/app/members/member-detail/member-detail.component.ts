import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryImage, NgxGalleryOptions } from '@kolkov/ngx-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import {NgxGalleryAnimation} from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { MessageService } from 'src/app/_services/message.service';
import { Pagination } from 'src/app/_models/pagination';
import { Message } from 'src/app/_models/message';


@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs',{static: true}) memberTabs: TabsetComponent;
  member: Member;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  activeTab: TabDirective;
  messages: Message[] =[];
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;
  loading = false;


  constructor(private memberService: MembersService,private route: ActivatedRoute, private messageService: MessageService) { }

  ngOnInit(): void {
  // this.loadmember();
  this.route.data.subscribe(data =>{
    this.member = data.member;
  })

  this.route.queryParams.subscribe(params =>{
    params.tab ? this.selectTab(params.tab) : this.selectTab(0);
  })

  this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false

      },
    ]

    this.galleryImages = this.getImages();

  }

  // loadmember(){
  //   this.memberService.getmember(this.route.snapshot.paramMap.get('username'))
  //   .subscribe(member => {
  //     this.member = member;
  //   })
  // }
    
  getImages():NgxGalleryImage[]{
    const imageUrls =[];
    for (const photo of this.member.photos){
      imageUrls.push({
        small:photo?.url,
        medium:photo?.url,
        big:photo?.url,
        
      })
    }
    return imageUrls;
  }

  loadmessages()
  {
    this.loading = true;
    this.messageService.getMessageThread(this.member.username,this.pageNumber,this.pageSize,)
    .subscribe(messages => {
      this.messages = messages.result;
      this.pagination = messages.pagination;
      this.loading = false;
      }
    )
  }

  pageChanged(event: any){
    if (this.pageNumber !== event.page) {
    this.pageNumber = event.page;
    this.loadmessages();
    }
  } 
    
  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.messages.length === 0){
      this.loadmessages();
    }
  }

  selectTab(tabId:number){
    this.memberTabs.tabs[tabId].active = true;
  }
}