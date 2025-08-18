// shared.module.ts
import { input, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PhoneValidPipe } from './pipes/phone-valid';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { TableModule } from 'primeng/table';
import { InputTextModule } from 'primeng/inputtext';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { PhoneFormatPipe } from './pipes/PhoneFormatPipe';
import { TimeAgoDetailedPipe } from './pipes/TimeAgoDetailedPipe';
import { ReplaceNbspPipe } from './pipes/ReplaceNbspPipe';
import { ShowMoreLessPipe } from './pipes/ShowMoreLessPipe';

@NgModule({
  declarations: [
    PhoneValidPipe,
    TimeAgoDetailedPipe,
    PhoneFormatPipe,
    ReplaceNbspPipe,
    ShowMoreLessPipe,
  ],
  imports: [
    CommonModule,
    NgxMaskDirective,
    TableModule,
    InputTextModule,
    BrowserModule,
    BrowserAnimationsModule,
  ],
  providers: [provideNgxMask()],  // Correct standalone mask provider
  exports: [
    PhoneValidPipe,
    TimeAgoDetailedPipe,
    PhoneFormatPipe,
    NgxMaskDirective,
    ReplaceNbspPipe,
    ShowMoreLessPipe,
  ]
})
export class SharedModule { }
