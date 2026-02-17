import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideHttpClient ,withInterceptors} from '@angular/common/http';
import { languageInterceptor } from './core/interceptors/language-interceptor';
import { provideTranslateService } from "@ngx-translate/core";
import { provideTranslateHttpLoader } from '@ngx-translate/http-loader'; 

import { routes } from './app.routes';
import { authInterceptor } from './core/interceptors/auth-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(withInterceptors([languageInterceptor, authInterceptor])),
    provideTranslateService({
      loader: provideTranslateHttpLoader({
        prefix: './core/assets/i18n/', // Your custom core path
        suffix: '.json'
      }),
      defaultLanguage: 'tr-TR'
    })
  ]
};