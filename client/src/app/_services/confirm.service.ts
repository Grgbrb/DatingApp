import { Injectable } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { observable, Observable } from 'rxjs';
import { ConfirmDialogComponent } from '../modals/confirm-dialog/confirm-dialog.component';

@Injectable({
  providedIn: 'root'
})
export class ConfirmService {
  BsModalRef: BsModalRef

  constructor(private modalService: BsModalService) { }

  confirm(title = 'Confrimation',
  message='Are you sure you want  to do this?',
  btnOkText='Yes', btnCancelText='No'): Observable<boolean>{
    const config = {
      initialState:{
        title,
        message,
        btnOkText,
        btnCancelText
      }
    }
    this.BsModalRef = this.modalService.show(ConfirmDialogComponent, config);

    return new Observable<boolean>(this.getResult());
  }

  private getResult(){
    return (observer) =>{
      const subscription = this.BsModalRef.onHidden.subscribe(() => {
        observer.next(this.BsModalRef.content.result);
        observer.complete();
      });

      return{
        unsubscribe(){
          subscription.unsubscribe();
        }
      }
    }
  }
}
