CREATE TABLE "tb_user" (
  "id" integer PRIMARY KEY,
  "name" varchar,
  "email" varchar,
  "password" varchar,
  "role_id" integer,
  "active" bool,
  "created_at" timestamp
);

CREATE TABLE "tb_role" (
  "id" integer PRIMARY KEY,
  "name" varchar,
  "description" varchar
);

ALTER TABLE "tb_user" ADD FOREIGN KEY ("role_id") REFERENCES "tb_role" ("id");
