// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { IEnvironment } from "./ienvironment";

export const environment: IEnvironment = {
  production: false,
  baseUrl: "https://localhost:4200/",
  apiBaseUrl: "https://localhost:5001/",
  stripePublicKey: "pk_test_51IjPWaCS7LyVv7pGg0hwInWOTVmzlSSjHeaZntg5sy4idOOGGUNSw4afw3XX7MGOcvoEfKmXqCCdFW0j11yGGw8W00qUGNsugB"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
