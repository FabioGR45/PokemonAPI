PT-BR:

Para executar o programa, siga uma das 2 opções:

1 - Em appsettings.Development.json, altere os seguintes valores:

"Secret": "base" - para "Secret": "18ccba186d8757c20cbf05d7a98b2c64f9f16eb64ea4a64659bbc5c9b7b3a7fe" | em TokenConfiguration

"login": "base" - para "login": "usuario" | em UserAuthentication
"senha": "base" - para "senha": "m1nh@s3nh@" | em UserAuthentication

2 - No arquivo secrets.json, adicione os seguintes valores:

  "UserAuthentication": {
    "login": "usuario",
    "senha": "m1nh@s3nh@"
  },
  "TokenConfiguration": {
    "Secret": "18ccba186d8757c20cbf05d7a98b2c64f9f16eb64ea4a64659bbc5c9b7b3a7fe"
  }

________________________________________________________________________________

No swagger, no endpoint ("Post") em Login, substitua:

{
  "username": "string",
  "password": "string"
}

para

{
    "login": "usuario",
    "senha": "m1nh@s3nh@"
}

-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

ING:

To run the program, do one of the 2 options:

1 - In appsettings.Development.json, change the following values:

"Secret": "base" - to "Secret": "18ccba186d8757c20cbf05d7a98b2c64f9f16eb64ea4a64659bbc5c9b7b3a7fe" | in TokenConfiguration

"login": "base" - to "login": "usuario" | in UserAuthentication
"senha": "base" - to "senha": "m1nh@s3nh@" | in UserAuthentication

2 - In the secrets.json file, add the following values:

  "UserAuthentication": {
    "login": "usuario",
    "senha": "m1nh@s3nh@"
  },
  "TokenConfiguration": {
    "Secret": "18ccba186d8757c20cbf05d7a98b2c64f9f16eb64ea4a64659bbc5c9b7b3a7fe"
  }

________________________________________________________________________________

In swagger, on the endpoint ("Post") in Login, replace:

{
  "username": "string",
  "password": "string"
}

to

{
    "login": "usuario",
    "senha": "m1nh@s3nh@"
}