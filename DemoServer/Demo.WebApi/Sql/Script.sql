-- Project sql scripts

-- DROP TABLE public.users;
CREATE TABLE public.users 
(
 user_id serial NOT NULL,
 first_name varchar(200) NOT NULL,
 last_name varchar(200) NOT NULL,
 birth_date date NULL,
 email varchar(200) NOT NULL,
 password_hash varchar(200) NOT NULL,
 register_date timestamp NOT NULL,
 CONSTRAINT users_pkey PRIMARY KEY (user_id)
);

-- DROP TABLE public.roles;
CREATE TABLE public.roles 
(
 role_id serial NOT NULL,
 role_name varchar(100) NOT NULL,
 add_date timestamp NOT NULL,
 CONSTRAINT roles_pk PRIMARY KEY (role_id)
);

-- DROP TABLE public.roles_distribution;
CREATE TABLE public.roles_distribution 
(
 roles_distribution_id serial NOT NULL,
 user_id int4 NOT NULL,
 role_id int4 NOT NULL,
 CONSTRAINT roles_distribution_pk PRIMARY KEY (roles_distribution_id)
);