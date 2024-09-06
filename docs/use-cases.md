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

## Update User

### Actors

- Admin
- Regular

### Definition

- The system must allow updating user information

### Rules

- Only admins can update information beyond his own
- The properties that can be updated are:
  - Full name
  - Email

## Update Password

### Actors

- Admin
- Regular

### Definition

- The system must allow password update

### Rules

- Only admins can update a password beyond his own
- All previous password rules applies to the update

## Deactivate User

### Actors

- Admin
- Regular

### Definition

- The system must allow users to deactivate their account
- The deactivation is made by setting the user's *activity status* property to *false*
- This property is set to *true* by default, at the moment of the user's creation

### Rules

- Only admins can deactivate a user beyond his own
- Only an active user can be deactivated
- After a specified amount of time **(still to be decided)** the deactivated user data is permanently deleted 

## Activate User

### Actors

- Admin
- Regular

### Definition

- The system must allow the activation of an inactive user

### Rules

- The activation can only happen in two cases:
  - An inactive user can ask for their account to be reactivated, and an admin must do so
  - A user tries to create a profile with an inactive user's e-mail
    - In this case he is asked if he wants to reactivate this account by provide some more information **(to be defined)**
- A user can only be reactivated happen before the already mentioned specified deletion time, after that the user data is permanently deleted

## Delete User

### Actors

- Admin
- Regular

### Definition

- The system must allow the permanent deletion of a users data

### Rules

- Only an inactive account can ben deleted
- The deletion only happens in to cases:
  - A user explicit asks for his data to be deleted
    - In this case an admin will delete the data
  - A user's data is automatically deleted after the already mentioned specified deletion time
