# CryptEx-Frontend

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 10.1.1.

## Technologies
Technologies we used and why:

### Angular
We picked Angular because we used it troughout the year and already have a lot of experience with it.
I kinda wanted to use Vue (shoutout to Renato) but we felt it was too risky to use a framework we never used.

### TailwindCSS
We picked TailwindCSS as our CSS framework as some of us already had some experience with it.

Though I never used it, I quickly knew how to do a lot of things with it, even tho I suck at CSS.

### ngx-translate
We used ngx-translate as our internationalization provider.
It was the most recommended package and it indeed was very easy to use.

### ngx-qrcode
I wanted to display a QR code when the users where depositing crypto-currencies.
I had some trouble choosing the best package for this task as some were too much hassle to use, or not fitting to Angular.

## Running locally

This project requires NodeJS to be installed (and thus NPM).
Before everything you must run 'npm install' in the current folder, so that dependencies get installed.

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

Add `--prod` when running the command above to use the Azure production server. (CORS is/should be configured properly so that you can use it 
when running locally).

## Common issues
Common issues you might encounter, and how to resolve them.

### Call stack out of memory
If you have an error that more or less says `Call stack out of memory` when running the project, try commenting the line 13 in `webpack.config.js`.

### No styling
If no styling is loaded (i.e all the pages are a big mess to look at), or you have any issue with CSS compilation,
then consider disabling Tailwind's JIT compilation in `tailwind.config.js`. The property name is 'jit', setting it to false will resolve this issue.

### I see a lot of red error messages
If you see a lot of errors, then go to the back-end server README and follow the *"Untrusted certificate"* issue resolution steps.

And make sure the back-end server is running in the first place ! :)


## Author
Laurent Keusch [<@laurentksh>](https://github.com/laurentksh) - 2021