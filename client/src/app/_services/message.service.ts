import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_models/message';
import { getPaginatedResult, getPaginationHeader } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }


  getMessages(pageNumber,pageSize,container)
  {
    let params = getPaginationHeader(pageNumber,pageSize);

    params = params.append('Container',container);

    return getPaginatedResult<Message[]>(this.baseUrl + 'messages',params, this.http);
  }

  getMessageThread(username,pageNumber,pageSize)
  {
    let params = getPaginationHeader(pageNumber,pageSize);

    return getPaginatedResult<Message[]>(this.baseUrl+ 'messages/thread/' + username, params, this.http); 
  }

  sendMessage(username: string, content: string)
  {
    return this.http.post<Message>(this.baseUrl + 'messages', {recipientUsername: username, content});
  }

  deleteMessage(id:number){
    return this.http.delete(this.baseUrl + 'messages/' + id );
  }

}
