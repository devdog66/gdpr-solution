# gdpr-solution
A Custom Consent Management approach for GDPR compliance

## Database script for MariaDB
-- Consents definition

CREATE TABLE `Consents` (
  `Id` bigint(20) NOT NULL AUTO_INCREMENT,
  `Uid` varchar(36) DEFAULT NULL,
  `LastUpdated` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `ConsentDate` datetime(6) NOT NULL DEFAULT current_timestamp(6),
  `CookiePreferences` varchar(5000) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) 

You can modify for the backend database you choose.

## Secret management
While the secrets are intended to go into some sort of Vault implementation, 
the current setup requires the user to create their own secrets.json
files to hold the secrets. These files have not been checked into github
and the user is responsible for creating these.

## Api Key Generation
The UnitTests project has a test named ApiKeyGenerator to generate 
api keys in Base64 format.

## Multiple Startup Projects
The solution is designed to startup both the Consent Service along with
one of the front end projects. The user will need to configure these
to their preferences.

## Vue JS Client app
The user is responsible for building this app and generating the contents
of the dist folder. See the README in the Web.Core/client-app folder for 
detailed instructions. 

