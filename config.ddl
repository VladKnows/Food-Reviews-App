-- Generated by Oracle SQL Developer Data Modeler 23.1.0.087.0806
--   at:        2024-01-08 02:15:20 EET
--   site:      Oracle Database 11g
--   type:      Oracle Database 11g



-- predefined type, no DDL - MDSYS.SDO_GEOMETRY

-- predefined type, no DDL - XMLTYPE

CREATE TABLE dishes (
    id_dish          NUMBER(8) NOT NULL,
    id_food_category NUMBER(4) NOT NULL,
    name             VARCHAR2(30) NOT NULL,
    ingredients      VARCHAR2(100)
);

ALTER TABLE dishes ADD CONSTRAINT dish_pk PRIMARY KEY ( id_dish );

ALTER TABLE dishes ADD CONSTRAINT name_u UNIQUE ( name );

ALTER TABLE dishes ADD CONSTRAINT ingredients_u UNIQUE ( ingredients );

CREATE TABLE food_categories (
    id_food_category NUMBER(4) NOT NULL,
    name             VARCHAR2(20) NOT NULL,
    description      VARCHAR2(50) NOT NULL
);

ALTER TABLE food_categories
    ADD CONSTRAINT food_category_name_ck CHECK ( REGEXP_LIKE ( name,
                                                               '^[A-Za-z ]*$' ) );

ALTER TABLE food_categories ADD CONSTRAINT food_category_pk PRIMARY KEY ( id_food_category );

ALTER TABLE food_categories ADD CONSTRAINT name UNIQUE ( name );

CREATE TABLE managers (
    id_manager     NUMBER(6) NOT NULL,
    name           VARCHAR2(35) NOT NULL,
    contact_number VARCHAR2(10) NOT NULL,
    mail_address   VARCHAR2(40) NOT NULL
);

ALTER TABLE managers
    ADD CONSTRAINT manager_name_ck CHECK ( REGEXP_LIKE ( name,
                                                         '^[A-Za-z ]*$' ) );

ALTER TABLE managers
    ADD CONSTRAINT contact_number_ck CHECK ( length(contact_number) = 10
                                             AND REGEXP_LIKE ( contact_number,
                                                               '^[0-9]+$' ) );

ALTER TABLE managers
    ADD CONSTRAINT email_ck CHECK ( REGEXP_LIKE ( mail_address,
                                                  '[a-z0-0._%-]+@[a-z0-9._%-]+\.[a-z]{2,4}' ) );

ALTER TABLE managers ADD CONSTRAINT managers_pk PRIMARY KEY ( id_manager );

ALTER TABLE managers ADD CONSTRAINT number_u UNIQUE ( contact_number );

ALTER TABLE managers ADD CONSTRAINT mail_u UNIQUE ( mail_address );

CREATE TABLE restaurant_dishes (
    id_restaurant_dishes NUMBER(5) NOT NULL,
    id_dish              NUMBER(8) NOT NULL,
    id_restaurant        NUMBER(6) NOT NULL,
    price                NUMBER(6, 2) NOT NULL,
    weight               NUMBER(4, 3)
);

ALTER TABLE restaurant_dishes ADD CONSTRAINT restaurant_dishes_pk PRIMARY KEY ( id_restaurant_dishes );

ALTER TABLE restaurant_dishes ADD CONSTRAINT restaurant_dishes__un UNIQUE ( id_dish,
                                                                            id_restaurant );

CREATE TABLE restaurants (
    id_restaurant NUMBER(6) NOT NULL,
    id_manager    NUMBER(6) NOT NULL,
    name          VARCHAR2(25) NOT NULL,
    address       VARCHAR2(60) NOT NULL
);

CREATE UNIQUE INDEX restaurants__idx ON
    restaurants (
        id_manager
    ASC );

ALTER TABLE restaurants ADD CONSTRAINT restaurants_pk PRIMARY KEY ( id_restaurant );

CREATE TABLE reviews (
    id_review            NUMBER(8) NOT NULL,
    id_user              NUMBER(8) NOT NULL,
    id_restaurant_dishes NUMBER(5) NOT NULL,
    score                NUMBER(2) NOT NULL,
    description          VARCHAR2(400)
);

ALTER TABLE reviews
    ADD CONSTRAINT review_ck CHECK ( score BETWEEN 1 AND 10 );

ALTER TABLE reviews ADD CONSTRAINT review_pk PRIMARY KEY ( id_review );

ALTER TABLE reviews ADD CONSTRAINT reviews__un UNIQUE ( id_user,
                                                        id_restaurant_dishes );

CREATE TABLE users (
    id_user      NUMBER(8) NOT NULL,
    name         VARCHAR2(35) NOT NULL,
    phone_number VARCHAR2(10) NOT NULL,
    points       NUMBER(4) DEFAULT 0 NOT NULL
);

ALTER TABLE users
    ADD CONSTRAINT user_name_ck CHECK ( REGEXP_LIKE ( name,
                                                      '^[A-Za-z ]*$' ) );

ALTER TABLE users
    ADD CONSTRAINT phone_number_ck CHECK ( length(phone_number) = 10
                                           AND REGEXP_LIKE ( phone_number,
                                                             '^[0-9]+$' ) );

ALTER TABLE users
    ADD CONSTRAINT points_ck CHECK ( points BETWEEN 0 AND 5000 );

ALTER TABLE users ADD CONSTRAINT user_pk PRIMARY KEY ( id_user );

ALTER TABLE users ADD CONSTRAINT phone_u UNIQUE ( phone_number );

ALTER TABLE restaurant_dishes
    ADD CONSTRAINT dish_rest_dish_fk FOREIGN KEY ( id_dish )
        REFERENCES dishes ( id_dish );

ALTER TABLE dishes
    ADD CONSTRAINT food_category_dish_fk FOREIGN KEY ( id_food_category )
        REFERENCES food_categories ( id_food_category );

ALTER TABLE restaurants
    ADD CONSTRAINT manager_restaurant_fk FOREIGN KEY ( id_manager )
        REFERENCES managers ( id_manager );

ALTER TABLE reviews
    ADD CONSTRAINT restaurant_dishes_fk FOREIGN KEY ( id_restaurant_dishes )
        REFERENCES restaurant_dishes ( id_restaurant_dishes );

ALTER TABLE restaurant_dishes
    ADD CONSTRAINT restaurant_rest_dish_fk FOREIGN KEY ( id_restaurant )
        REFERENCES restaurants ( id_restaurant );

ALTER TABLE reviews
    ADD CONSTRAINT user_review_fk FOREIGN KEY ( id_user )
        REFERENCES users ( id_user );

CREATE SEQUENCE dishes_id_dish_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER dishes_id_dish_trg BEFORE
    INSERT ON dishes
    FOR EACH ROW
    WHEN ( new.id_dish IS NULL )
BEGIN
    :new.id_dish := dishes_id_dish_seq.nextval;
END;
/

CREATE SEQUENCE food_categories_id_food_catego START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER food_categories_id_food_catego BEFORE
    INSERT ON food_categories
    FOR EACH ROW
    WHEN ( new.id_food_category IS NULL )
BEGIN
    :new.id_food_category := food_categories_id_food_catego.nextval;
END;
/

CREATE SEQUENCE managers_id_manager_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER managers_id_manager_trg BEFORE
    INSERT ON managers
    FOR EACH ROW
    WHEN ( new.id_manager IS NULL )
BEGIN
    :new.id_manager := managers_id_manager_seq.nextval;
END;
/

CREATE SEQUENCE restaurant_dishes_id_restauran START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER restaurant_dishes_id_restauran BEFORE
    INSERT ON restaurant_dishes
    FOR EACH ROW
    WHEN ( new.id_restaurant_dishes IS NULL )
BEGIN
    :new.id_restaurant_dishes := restaurant_dishes_id_restauran.nextval;
END;
/

CREATE SEQUENCE restaurants_id_restaurant_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER restaurants_id_restaurant_trg BEFORE
    INSERT ON restaurants
    FOR EACH ROW
    WHEN ( new.id_restaurant IS NULL )
BEGIN
    :new.id_restaurant := restaurants_id_restaurant_seq.nextval;
END;
/

CREATE SEQUENCE reviews_id_review_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER reviews_id_review_trg BEFORE
    INSERT ON reviews
    FOR EACH ROW
    WHEN ( new.id_review IS NULL )
BEGIN
    :new.id_review := reviews_id_review_seq.nextval;
END;
/

CREATE SEQUENCE users_id_user_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER users_id_user_trg BEFORE
    INSERT ON users
    FOR EACH ROW
    WHEN ( new.id_user IS NULL )
BEGIN
    :new.id_user := users_id_user_seq.nextval;
END;
/



-- Oracle SQL Developer Data Modeler Summary Report: 
-- 
-- CREATE TABLE                             7
-- CREATE INDEX                             1
-- ALTER TABLE                             29
-- CREATE VIEW                              0
-- ALTER VIEW                               0
-- CREATE PACKAGE                           0
-- CREATE PACKAGE BODY                      0
-- CREATE PROCEDURE                         0
-- CREATE FUNCTION                          0
-- CREATE TRIGGER                           7
-- ALTER TRIGGER                            0
-- CREATE COLLECTION TYPE                   0
-- CREATE STRUCTURED TYPE                   0
-- CREATE STRUCTURED TYPE BODY              0
-- CREATE CLUSTER                           0
-- CREATE CONTEXT                           0
-- CREATE DATABASE                          0
-- CREATE DIMENSION                         0
-- CREATE DIRECTORY                         0
-- CREATE DISK GROUP                        0
-- CREATE ROLE                              0
-- CREATE ROLLBACK SEGMENT                  0
-- CREATE SEQUENCE                          7
-- CREATE MATERIALIZED VIEW                 0
-- CREATE MATERIALIZED VIEW LOG             0
-- CREATE SYNONYM                           0
-- CREATE TABLESPACE                        0
-- CREATE USER                              0
-- 
-- DROP TABLESPACE                          0
-- DROP DATABASE                            0
-- 
-- REDACTION POLICY                         0
-- 
-- ORDS DROP SCHEMA                         0
-- ORDS ENABLE SCHEMA                       0
-- ORDS ENABLE OBJECT                       0
-- 
-- ERRORS                                   0
-- WARNINGS                                 0
