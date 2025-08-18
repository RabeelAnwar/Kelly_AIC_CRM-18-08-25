import { Injectable } from '@angular/core';
import { Essentials, Paragraph, Bold, Italic, Link, Underline, Superscript, Strikethrough, BlockQuote, Alignment, List, Font, FontSize, FontColor, Subscript, PasteFromOffice, ClassicEditor } from 'ckeditor5';

@Injectable({
  providedIn: 'root'
})
export class CkeditorConfigService {

  constructor() { }

    public Editor = ClassicEditor;
    public config = {
      licenseKey: 'GPL', // Or 'GPL'.
      plugins: [
  
        Essentials,
        Paragraph,
        Bold,
        Italic,
        Underline,
        Strikethrough,
        Link,
        BlockQuote,
        Alignment,
        List,
        Font,
        FontSize,
        FontColor,
        Subscript,
        Superscript,
        PasteFromOffice
      ],
      toolbar: ['undo', 'redo', '|', 'bold', 'italic', '|', 'link', 'underline',
        'subscript', 'superscript'
        , 'strikethrough', '|', 'alignment', 'bulletedList', 'numberedList', '|', 'fontSize', 'fontColor', 'blockQuote', 'pasteFromOffice'],
    }

}
