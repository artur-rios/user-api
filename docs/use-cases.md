# Use cases

## Create User

### Actors

- Admin
- Regular

### Definition

- The system must allow the creation of users

### Rules

- Only admins can create users beyond his own profile
- Only admins can create users with the roles *Admin* and *Test*
- The e-mail of a user is unique

#### Property rules

- A user can only be created if all properties are provided and valid
    - Except *creation date* and *activity status* those are system controlled properties
- An e-mail is valid if it follows all the specifications of an e-mail address
- A name is valid if it's not null or empty
- A password is valid if it has at least:
  - 8 characters
  - An upper case letter
  - A lower case letter
  - A number
- The password must not be stored in full text, only as a hash
- A user must have one of the available roles (*Admin*, *Regular* and *Test*)
- A user must have a *creation date* calculated by the system
- A user must have an *activity status* that is always true when the user is created

## Read User

### Actors

- Admin
- Regular

### Definition

- The system must allow consulting all information of all users

### Rules

- Only admins can read information beyond his own
