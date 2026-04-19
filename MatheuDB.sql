--
-- PostgreSQL database dump
--

\restrict tSgdDtlGQQsqRkKpxVS3atT8nKMggehgRsI3beopfQU5UIDFW57Qp3JZJCtJgZe

-- Dumped from database version 18.0
-- Dumped by pg_dump version 18.0

-- Started on 2026-04-19 09:17:40

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 224 (class 1259 OID 20303)
-- Name: Categories; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Categories" (
    "CategoryId" integer NOT NULL,
    "Name" character varying(100) NOT NULL,
    "Description" text,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 223 (class 1259 OID 20302)
-- Name: Categories_CategoryId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Categories_CategoryId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4993 (class 0 OID 0)
-- Dependencies: 223
-- Name: Categories_CategoryId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Categories_CategoryId_seq" OWNED BY public."Categories"."CategoryId";


--
-- TOC entry 232 (class 1259 OID 20371)
-- Name: OrderItems; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."OrderItems" (
    "OrderItemId" integer NOT NULL,
    "OrderId" integer NOT NULL,
    "ProductId" integer NOT NULL,
    "Quantity" integer DEFAULT 1 NOT NULL,
    "UnitPrice" numeric(10,2) NOT NULL,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 231 (class 1259 OID 20370)
-- Name: OrderItems_OrderItemId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."OrderItems_OrderItemId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4994 (class 0 OID 0)
-- Dependencies: 231
-- Name: OrderItems_OrderItemId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."OrderItems_OrderItemId_seq" OWNED BY public."OrderItems"."OrderItemId";


--
-- TOC entry 230 (class 1259 OID 20352)
-- Name: Orders; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Orders" (
    "OrderId" integer NOT NULL,
    "UserId" integer NOT NULL,
    "OrderDate" timestamp without time zone DEFAULT now() NOT NULL,
    "Status" character varying(50) DEFAULT 'Pending'::character varying NOT NULL,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 229 (class 1259 OID 20351)
-- Name: Orders_OrderId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Orders_OrderId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4995 (class 0 OID 0)
-- Dependencies: 229
-- Name: Orders_OrderId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Orders_OrderId_seq" OWNED BY public."Orders"."OrderId";


--
-- TOC entry 228 (class 1259 OID 20325)
-- Name: Products; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Products" (
    "ProductId" integer NOT NULL,
    "Name" character varying(150) NOT NULL,
    "Price" numeric(10,2) NOT NULL,
    "Stock" integer DEFAULT 0 NOT NULL,
    "ImagePath" character varying(300),
    "Collection" character varying(100),
    "CategoryId" integer NOT NULL,
    "SupplierId" integer NOT NULL,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 227 (class 1259 OID 20324)
-- Name: Products_ProductId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Products_ProductId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4996 (class 0 OID 0)
-- Dependencies: 227
-- Name: Products_ProductId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Products_ProductId_seq" OWNED BY public."Products"."ProductId";


--
-- TOC entry 220 (class 1259 OID 20270)
-- Name: Roles; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Roles" (
    "RoleId" integer NOT NULL,
    "Name" character varying(50) NOT NULL,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 219 (class 1259 OID 20269)
-- Name: Roles_RoleId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Roles_RoleId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4997 (class 0 OID 0)
-- Dependencies: 219
-- Name: Roles_RoleId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Roles_RoleId_seq" OWNED BY public."Roles"."RoleId";


--
-- TOC entry 226 (class 1259 OID 20315)
-- Name: Suppliers; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Suppliers" (
    "SupplierId" integer NOT NULL,
    "CompanyName" character varying(150) NOT NULL,
    "ContactName" character varying(100),
    "Phone" character varying(30),
    "Email" character varying(100),
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 225 (class 1259 OID 20314)
-- Name: Suppliers_SupplierId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Suppliers_SupplierId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4998 (class 0 OID 0)
-- Dependencies: 225
-- Name: Suppliers_SupplierId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Suppliers_SupplierId_seq" OWNED BY public."Suppliers"."SupplierId";


--
-- TOC entry 222 (class 1259 OID 20282)
-- Name: Users; Type: TABLE; Schema: public; Owner: -
--

CREATE TABLE public."Users" (
    "UserId" integer NOT NULL,
    "Login" character varying(100) NOT NULL,
    "Password" character varying(255) NOT NULL,
    "FullName" character varying(150),
    "Email" character varying(100),
    "RoleId" integer NOT NULL,
    "UpdatedAt" timestamp without time zone DEFAULT now()
);


--
-- TOC entry 221 (class 1259 OID 20281)
-- Name: Users_UserId_seq; Type: SEQUENCE; Schema: public; Owner: -
--

CREATE SEQUENCE public."Users_UserId_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


--
-- TOC entry 4999 (class 0 OID 0)
-- Dependencies: 221
-- Name: Users_UserId_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: -
--

ALTER SEQUENCE public."Users_UserId_seq" OWNED BY public."Users"."UserId";


--
-- TOC entry 4789 (class 2604 OID 20306)
-- Name: Categories CategoryId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Categories" ALTER COLUMN "CategoryId" SET DEFAULT nextval('public."Categories_CategoryId_seq"'::regclass);


--
-- TOC entry 4800 (class 2604 OID 20374)
-- Name: OrderItems OrderItemId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderItems" ALTER COLUMN "OrderItemId" SET DEFAULT nextval('public."OrderItems_OrderItemId_seq"'::regclass);


--
-- TOC entry 4796 (class 2604 OID 20355)
-- Name: Orders OrderId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Orders" ALTER COLUMN "OrderId" SET DEFAULT nextval('public."Orders_OrderId_seq"'::regclass);


--
-- TOC entry 4793 (class 2604 OID 20328)
-- Name: Products ProductId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Products" ALTER COLUMN "ProductId" SET DEFAULT nextval('public."Products_ProductId_seq"'::regclass);


--
-- TOC entry 4785 (class 2604 OID 20273)
-- Name: Roles RoleId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Roles" ALTER COLUMN "RoleId" SET DEFAULT nextval('public."Roles_RoleId_seq"'::regclass);


--
-- TOC entry 4791 (class 2604 OID 20318)
-- Name: Suppliers SupplierId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Suppliers" ALTER COLUMN "SupplierId" SET DEFAULT nextval('public."Suppliers_SupplierId_seq"'::regclass);


--
-- TOC entry 4787 (class 2604 OID 20285)
-- Name: Users UserId; Type: DEFAULT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users" ALTER COLUMN "UserId" SET DEFAULT nextval('public."Users_UserId_seq"'::regclass);


--
-- TOC entry 4979 (class 0 OID 20303)
-- Dependencies: 224
-- Data for Name: Categories; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Categories" VALUES (1, 'Кастом', 'Кастомные изделия ручной работы', '2026-04-19 02:37:25.968195');
INSERT INTO public."Categories" VALUES (2, 'Косплей', 'Косплей костюмы и аксессуары', '2026-04-19 02:37:25.968195');


--
-- TOC entry 4987 (class 0 OID 20371)
-- Dependencies: 232
-- Data for Name: OrderItems; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."OrderItems" VALUES (1, 1, 1, 1, 3500.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (2, 2, 6, 1, 5500.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (3, 3, 2, 1, 4200.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (4, 4, 7, 1, 6200.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (5, 5, 3, 2, 2800.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (6, 6, 8, 1, 2900.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (7, 7, 4, 3, 1500.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (8, 8, 9, 2, 1800.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (9, 9, 5, 1, 3200.00, '2026-04-19 02:37:25.968195');
INSERT INTO public."OrderItems" VALUES (10, 10, 10, 1, 2100.00, '2026-04-19 02:37:25.968195');


--
-- TOC entry 4985 (class 0 OID 20352)
-- Dependencies: 230
-- Data for Name: Orders; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Orders" VALUES (1, 4, '2025-01-05 10:00:00', 'Delivered', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (2, 5, '2025-01-12 11:30:00', 'Delivered', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (3, 6, '2025-02-03 09:15:00', 'Shipped', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (4, 7, '2025-02-18 14:00:00', 'Shipped', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (5, 8, '2025-03-07 16:45:00', 'Processing', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (6, 9, '2025-03-20 08:30:00', 'Processing', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (7, 10, '2025-04-01 12:00:00', 'Pending', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (8, 4, '2025-04-10 13:20:00', 'Pending', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (9, 5, '2025-04-14 17:00:00', 'Cancelled', '2026-04-19 02:37:25.968195');
INSERT INTO public."Orders" VALUES (10, 6, '2025-04-17 10:10:00', 'Pending', '2026-04-19 02:37:25.968195');


--
-- TOC entry 4983 (class 0 OID 20325)
-- Dependencies: 228
-- Data for Name: Products; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Products" VALUES (1, 'Кастомные кроссовки Nike', 3500.00, 15, 'Ресурсы/pr/Кастом/Pr1.jpg', 'Киберпанк', 1, 1, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (2, 'Кастомная куртка с принтом', 4200.00, 10, 'Ресурсы/pr/Кастом/Pr2.jpg', 'Нуар', 1, 6, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (3, 'Кастомная сумка ручной работы', 2800.00, 20, 'Ресурсы/pr/Кастом/Pr3.jpg', 'Аниме', 1, 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (4, 'Кастомная футболка аниме', 1500.00, 30, 'Ресурсы/pr/Кастом/Pr4.jpg', 'Аниме', 1, 4, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (5, 'Кастомный рюкзак с вышивкой', 3200.00, 12, 'Ресурсы/pr/Кастом/Pr5.jpg', 'Новый год', 1, 8, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (6, 'Косплей костюм Наруто', 5500.00, 8, 'Ресурсы/pr/Косплей/KL1.jpg', 'Аниме', 2, 5, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (7, 'Косплей костюм Сейлор Мун', 6200.00, 6, 'Ресурсы/pr/Косплей/KL2.jpg', 'Аниме', 2, 9, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (8, 'Косплей меч Клинок рассекающий', 2900.00, 14, 'Ресурсы/pr/Косплей/KL3.jpg', 'Аниме', 2, 7, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (9, 'Косплей маска Тоторо', 1800.00, 25, 'Ресурсы/pr/Косплей/KL4.jpg', 'Хэллоуин', 2, 2, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (10, 'Косплей парик Микаса', 2100.00, 18, 'Ресурсы/pr/Косплей/KL5.jpg', 'Аниме', 2, 10, '2026-04-19 02:37:25.968195');
INSERT INTO public."Products" VALUES (11, 'Футболка для Карима (Спецзаказ)', 11000.00, 1, 'C:\Users\glnllz\Desktop\MathieuShop-master\MathieuShop\Ресурсы\pr\Кастом\Pr10.jpg', 'Киберпанк', 2, 10, '2026-04-19 05:12:08.276451');


--
-- TOC entry 4975 (class 0 OID 20270)
-- Dependencies: 220
-- Data for Name: Roles; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Roles" VALUES (1, 'Admin', '2026-04-19 02:37:25.968195');
INSERT INTO public."Roles" VALUES (2, 'Manager', '2026-04-19 02:37:25.968195');
INSERT INTO public."Roles" VALUES (3, 'Customer', '2026-04-19 02:37:25.968195');


--
-- TOC entry 4981 (class 0 OID 20315)
-- Dependencies: 226
-- Data for Name: Suppliers; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Suppliers" VALUES (1, 'ArtCraft Studio', 'Elena Petrova', '+7-900-001-01-01', 'elena@artcraft.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (2, 'CosWorld Ltd', 'Dmitry Ivanov', '+7-900-001-01-02', 'dmitry@cosworld.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (3, 'HandMade Pro', 'Maria Sidorova', '+7-900-001-01-03', 'maria@handmade.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (4, 'FabricMaster', 'Alexey Kozlov', '+7-900-001-01-04', 'alexey@fabric.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (5, 'CosplayHub', 'Natalia Volkova', '+7-900-001-01-05', 'natalia@coshub.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (6, 'CustomKing', 'Igor Sokolov', '+7-900-001-01-06', 'igor@customking.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (7, 'PropFactory', 'Olga Novikova', '+7-900-001-01-07', 'olga@propfactory.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (8, 'SewingAtelier', 'Pavel Morozov', '+7-900-001-01-08', 'pavel@atelier.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (9, 'AnimeCraft', 'Yulia Fedorova', '+7-900-001-01-09', 'yulia@animecraft.ru', '2026-04-19 02:37:25.968195');
INSERT INTO public."Suppliers" VALUES (10, 'MasterPiece Co', 'Sergey Lebedev', '+7-900-001-01-10', 'sergey@masterpiece.ru', '2026-04-19 02:37:25.968195');


--
-- TOC entry 4977 (class 0 OID 20282)
-- Dependencies: 222
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: -
--

INSERT INTO public."Users" VALUES (1, 'admin', 'admin123', 'Mathieu Admin', 'admin@mathieushop.com', 1, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (2, 'manager1', 'manager123', 'Alice Johnson', 'alice@mathieushop.com', 2, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (3, 'manager2', 'manager123', 'Bob Martinez', 'bob@mathieushop.com', 2, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (4, 'customer1', 'pass123', 'Liam Smith', 'liam.smith@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (5, 'customer2', 'pass123', 'Emma Jones', 'emma.jones@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (6, 'customer3', 'pass123', 'Noah Williams', 'noah.williams@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (7, 'customer4', 'pass123', 'Olivia Brown', 'olivia.brown@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (8, 'customer5', 'pass123', 'William Davis', 'william.davis@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (9, 'customer6', 'pass123', 'Ava Miller', 'ava.miller@email.com', 3, '2026-04-19 02:37:25.968195');
INSERT INTO public."Users" VALUES (10, 'customer7', 'pass123', 'James Wilson', 'james.wilson@email.com', 3, '2026-04-19 02:37:25.968195');


--
-- TOC entry 5000 (class 0 OID 0)
-- Dependencies: 223
-- Name: Categories_CategoryId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Categories_CategoryId_seq"', 2, true);


--
-- TOC entry 5001 (class 0 OID 0)
-- Dependencies: 231
-- Name: OrderItems_OrderItemId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."OrderItems_OrderItemId_seq"', 10, true);


--
-- TOC entry 5002 (class 0 OID 0)
-- Dependencies: 229
-- Name: Orders_OrderId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Orders_OrderId_seq"', 10, true);


--
-- TOC entry 5003 (class 0 OID 0)
-- Dependencies: 227
-- Name: Products_ProductId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Products_ProductId_seq"', 11, true);


--
-- TOC entry 5004 (class 0 OID 0)
-- Dependencies: 219
-- Name: Roles_RoleId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Roles_RoleId_seq"', 3, true);


--
-- TOC entry 5005 (class 0 OID 0)
-- Dependencies: 225
-- Name: Suppliers_SupplierId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Suppliers_SupplierId_seq"', 10, true);


--
-- TOC entry 5006 (class 0 OID 0)
-- Dependencies: 221
-- Name: Users_UserId_seq; Type: SEQUENCE SET; Schema: public; Owner: -
--

SELECT pg_catalog.setval('public."Users_UserId_seq"', 10, true);


--
-- TOC entry 4812 (class 2606 OID 20313)
-- Name: Categories Categories_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Categories"
    ADD CONSTRAINT "Categories_pkey" PRIMARY KEY ("CategoryId");


--
-- TOC entry 4820 (class 2606 OID 20383)
-- Name: OrderItems OrderItems_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "OrderItems_pkey" PRIMARY KEY ("OrderItemId");


--
-- TOC entry 4818 (class 2606 OID 20364)
-- Name: Orders Orders_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "Orders_pkey" PRIMARY KEY ("OrderId");


--
-- TOC entry 4816 (class 2606 OID 20340)
-- Name: Products Products_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "Products_pkey" PRIMARY KEY ("ProductId");


--
-- TOC entry 4804 (class 2606 OID 20280)
-- Name: Roles Roles_Name_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "Roles_Name_key" UNIQUE ("Name");


--
-- TOC entry 4806 (class 2606 OID 20278)
-- Name: Roles Roles_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Roles"
    ADD CONSTRAINT "Roles_pkey" PRIMARY KEY ("RoleId");


--
-- TOC entry 4814 (class 2606 OID 20323)
-- Name: Suppliers Suppliers_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Suppliers"
    ADD CONSTRAINT "Suppliers_pkey" PRIMARY KEY ("SupplierId");


--
-- TOC entry 4808 (class 2606 OID 20296)
-- Name: Users Users_Login_key; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_Login_key" UNIQUE ("Login");


--
-- TOC entry 4810 (class 2606 OID 20294)
-- Name: Users Users_pkey; Type: CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_pkey" PRIMARY KEY ("UserId");


--
-- TOC entry 4825 (class 2606 OID 20384)
-- Name: OrderItems OrderItems_OrderId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "OrderItems_OrderId_fkey" FOREIGN KEY ("OrderId") REFERENCES public."Orders"("OrderId") ON DELETE CASCADE;


--
-- TOC entry 4826 (class 2606 OID 20389)
-- Name: OrderItems OrderItems_ProductId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."OrderItems"
    ADD CONSTRAINT "OrderItems_ProductId_fkey" FOREIGN KEY ("ProductId") REFERENCES public."Products"("ProductId");


--
-- TOC entry 4824 (class 2606 OID 20365)
-- Name: Orders Orders_UserId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Orders"
    ADD CONSTRAINT "Orders_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES public."Users"("UserId");


--
-- TOC entry 4822 (class 2606 OID 20341)
-- Name: Products Products_CategoryId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "Products_CategoryId_fkey" FOREIGN KEY ("CategoryId") REFERENCES public."Categories"("CategoryId");


--
-- TOC entry 4823 (class 2606 OID 20346)
-- Name: Products Products_SupplierId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Products"
    ADD CONSTRAINT "Products_SupplierId_fkey" FOREIGN KEY ("SupplierId") REFERENCES public."Suppliers"("SupplierId");


--
-- TOC entry 4821 (class 2606 OID 20297)
-- Name: Users Users_RoleId_fkey; Type: FK CONSTRAINT; Schema: public; Owner: -
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "Users_RoleId_fkey" FOREIGN KEY ("RoleId") REFERENCES public."Roles"("RoleId");


-- Completed on 2026-04-19 09:17:40

--
-- PostgreSQL database dump complete
--

\unrestrict tSgdDtlGQQsqRkKpxVS3atT8nKMggehgRsI3beopfQU5UIDFW57Qp3JZJCtJgZe

