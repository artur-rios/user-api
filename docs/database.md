# Database

Relational database, used to store register info from users

## DBML Definition

- [DBML documentation](https://dbml.dbdiagram.io/docs)

```sql
Table tb_user {
  id integer [primary key, increment]
  name varchar [not null]
  email varchar [unique, not null]
  password varchar [not null]
  salt varchar [not null]
  role_id integer [not null]
  active bool [not null]
  created_at timestamp [not null]
}

Table tb_role {
  id integer [primary key, increment]
  name varchar [unique, not null]
  description varchar [not null]
}

Ref: tb_user.role_id > tb_role.id  
```
