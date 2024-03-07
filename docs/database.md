# Database

Relational database, used to store register info from users

## DBML Definition

- [DBML documentation](https://dbml.dbdiagram.io/docs)

```sql
Table tb_user {
  id integer [primary key]
  name varchar
  email varchar
  password varchar
  role_id integer
  active bool
  created_at timestamp 
}

Table tb_role {
  id integer [primary key]
  name varchar
  description varchar
}

Ref: tb_user.role_id > tb_role.id 
```
