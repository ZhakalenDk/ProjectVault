# Introduction
Project vault is an assignment given during the H4 course _Software Testing and Security_. \
The program is designed to demonstrate the following 3 topics:
1. **Hashing**
1. **Decryption/Encryption**
1. **Unit Testing**

In that context the program isn't secure, as it's purely conceptional and not meant useful
in any form other than a presentation on the 3 topics.

## How To
**Project Vault** can create new users and store their details in a text file.
The password are hashed and is unique to each user, even if they have the same password,
as the salt used to hash passwords is based on the username of the user (_usernames must be unique_).

The way it works is that the user signs into their _account_ where they will be
prompted to enter a path and a filename for a file they want to encrypt.
The program generates the encrypted version of the specified file at the same directory
as the original, however, the encrypted file is called `Encrypted_[filename]`.

The password is saved with the file and as such only the user that encrypted the file
will be able to decrypt it.
This is of course only the idea. In reality the method used to accomplish this
in the program is fairly simple and probably easy to crack, however, for the purpose of
the demenstration this serves it's purpose quite well.

Only one file can be encrypted by a user at a time, if another is ecnrypted the stored
`Private key` for that user is overwritten.
One way to work around that is to create a user for each file encrrypted this way.

**Note:** Originally the program was meant to be a small messanger, sending encrypted
messages between two accounts, however, due to lack of time the idea was abandoned.

# Project Details
| Platform       | UI          | Timeframe | Database Solution        |
|----------------|-------------|-----------|--------------------------|
| Windows | Console | | |

See the [WiKi](./WikiPages/Front.md) for more in depth information about the project.

## Initial Features
- [X] User Handling (With sign in)
- [X] Encryption/Decryption of files
- [X] Password hashing

## Change Log
_`Classes`, `structs`, `interfaces` and `enums` etc. are to be linked to the project WiKi Page._
