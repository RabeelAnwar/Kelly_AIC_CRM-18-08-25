import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SerialPortService {
  dataSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');

  constructor() { }
    
  async connectToPort() { debugger
    const nav: any = navigator;
    const port = await nav.serial.requestPort();
    await port.open({ baudRate: 9600,
      dataBits: 8,
      parity: 'none',
      stopBits: 1,
    });
     
    const reader = port.readable.getReader();
   
    while (true) {
      const { value, done } = await reader.read();
      if (done) break;
      const text = new TextDecoder().decode(value);
      this.dataSubject.next(text);
      console.log(text);
    }
  }
}
