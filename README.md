# CryptEx
Welcome to the repository for our school project 'CryptEx'.
Here you will find informations about the project and how to run.

**EPSIC 2021 - ICT 183 + 150 + 133**

## Information
Some informations on the project

### Project summary
This is a 'crypto broker/exchange platform' website.

By creating an account, our users can deposit fiat (i.e: real currencies, like USD) and trade it in exchange of crypto-currencies, like Bitcoin,
Doge or Ethereum.

Crypto-currencies exchange rates are provided by Coinbase, through their "basic" API.

We offer a premium subscription which mainly removes users' transaction fees.

Through the admin portal, we (owners) can manually manage user accounts: disabling their accounts, approving or declining their bank accounts.

After trading on the platform, the users can then withdraw their money on their bank account.

### Technologies used
Please read project-specific READMEs for more informations on the technologies we used.

## Sub projects
This project has two sub-projects, "CryptExApi" and "CryptExFrontEnd" :

### CryptExApi

The back-end service, made with C#/ASP.NET Core.
It uses Entity Framework Core with a MS-SQL database.

More in-depth documentation is available in [src/back-end/CryptEx/README.md](https://github.com/laurentksh/CryptEx/blob/master/src/back-end/CryptEx/README.md)


### CryptExFrontEnd

The front-end interface, made with Angular/TypeScript.

More in-depth documentation is available in [src/front-end/CryptExFrontEnd/README.md](https://github.com/laurentksh/CryptEx/blob/master/src/front-end/CryptExFrontEnd/README.md)


## Contributing
We do not accept contributions as this is a school project (except from our teachers and classmates obviously!)


## Authors & Work attribution

- Laurent Keusch [<@laurentksh>](https://github.com/laurentksh)
  - Made 100% of the back-end. Made the admin, asset-convert, auth, deposit-withdraw, user and wallet modules.
- Luca Landry [<@landryl986>](https://github.com/landryl986)
  - Worked on the user, auth and premium module.
- Ethan Marchand [<@Ethan0079>](https://github.com/Ethan0079)
  - Made the main components (home, contact, error pages, etc.).

# Attributions
This project does not have a license (i.e: Copyright)


Third-party libraries (probably) requiring attributions:

- HeroIcons (MIT Licensed)
- TailwindUI (Unknown license, tailwindui.com)

... And probably some more we forgot to include.