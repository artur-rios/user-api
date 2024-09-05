# Actors

 - The system will be used by the following actors:

## Admin

- Has access to all system features

## Regular

- Has access to the following features:
  - Create user (only for himself)
  - Authenticate
  - Read user (only his own profile)
  - Deactivate his on profile

## Test

- A special role used only for functional tests
- Can only be created by an admin
- Cannot be used outside the test environment
- Has access to all system features, except create an admin
