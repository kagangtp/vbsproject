import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import trTranslations from './core/assets/i18n/tr-TR.json';
import enTranslations from './core/assets/i18n/en-US.json';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected title = 'UI';

  constructor(private translate: TranslateService) {
    // 1. Set the default fallback language in case a translation is missing
    this.translate.setFallbackLang('en-US');
  }

  ngOnInit(): void {
    // 2. Read the same key your interceptor uses
    const savedLang = localStorage.getItem('language') || 'en-US';

    // 3. Tell the UI to load the corresponding JSON file
    this.translate.use(savedLang);
  }
}