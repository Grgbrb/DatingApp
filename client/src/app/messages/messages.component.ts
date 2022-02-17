import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages: Message[];
  pagination: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadmessages();
  }

  loadmessages(){
    this.loading = true;
    this.messageService.getMessages(this.pageNumber,this.pageSize,this.container)
    .subscribe(messages =>{
      this.messages = messages.result;
      this.pagination = messages.pagination;
      this.loading = false;
    })
  }

  deleteMessage(id: number){
    this.messageService.deleteMessage(id).subscribe(() => {
      this.messages.splice(this.messages.findIndex(m => m.id === id), 1);
    })
  } 

  pageChanged(event: any){
    if (this.pageNumber !== event.page) {
    this.pageNumber = event.page;
    this.loadmessages();
    }
  }

}
