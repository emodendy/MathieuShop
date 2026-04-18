-- ============================================================
-- MathieuShopDB - PostgreSQL Database Script
-- ============================================================

-- ============================================================
-- DROP TABLES (cascade, for re-run)
-- ============================================================
DROP TABLE IF EXISTS "OrderItems"  CASCADE;
DROP TABLE IF EXISTS "Orders"      CASCADE;
DROP TABLE IF EXISTS "Products"    CASCADE;
DROP TABLE IF EXISTS "Categories"  CASCADE;
DROP TABLE IF EXISTS "Suppliers"   CASCADE;
DROP TABLE IF EXISTS "Users"       CASCADE;
DROP TABLE IF EXISTS "Roles"       CASCADE;
-- ============================================================
-- TABLE: Roles
-- ============================================================
CREATE TABLE "Roles" (
    "RoleId"      SERIAL PRIMARY KEY,
    "Name"        VARCHAR(50) NOT NULL UNIQUE,
    "UpdatedAt"   TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: Users
-- ============================================================
CREATE TABLE "Users" (
    "UserId"      SERIAL PRIMARY KEY,
    "Login"       VARCHAR(100) NOT NULL UNIQUE,
    "Password"    VARCHAR(255) NOT NULL,
    "FullName"    VARCHAR(150),
    "Email"       VARCHAR(100),
    "RoleId"      INT NOT NULL REFERENCES "Roles"("RoleId"),
    "UpdatedAt"   TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: Categories
-- ============================================================
CREATE TABLE "Categories" (
    "CategoryId"   SERIAL PRIMARY KEY,
    "Name"         VARCHAR(100) NOT NULL,
    "Description"  TEXT,
    "UpdatedAt"    TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: Suppliers
-- ============================================================
CREATE TABLE "Suppliers" (
    "SupplierId"   SERIAL PRIMARY KEY,
    "CompanyName"  VARCHAR(150) NOT NULL,
    "ContactName"  VARCHAR(100),
    "Phone"        VARCHAR(30),
    "Email"        VARCHAR(100),
    "UpdatedAt"    TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: Products
-- ============================================================
CREATE TABLE "Products" (
    "ProductId"    SERIAL PRIMARY KEY,
    "Name"         VARCHAR(150) NOT NULL,
    "Price"        NUMERIC(10,2) NOT NULL,
    "Stock"        INT NOT NULL DEFAULT 0,
    "ImagePath"    VARCHAR(300),
    "Collection"   VARCHAR(100),
    "CategoryId"   INT NOT NULL REFERENCES "Categories"("CategoryId"),
    "SupplierId"   INT NOT NULL REFERENCES "Suppliers"("SupplierId"),
    "UpdatedAt"    TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: Orders
-- ============================================================
CREATE TABLE "Orders" (
    "OrderId"      SERIAL PRIMARY KEY,
    "UserId"       INT NOT NULL REFERENCES "Users"("UserId"),
    "OrderDate"    TIMESTAMP NOT NULL DEFAULT NOW(),
    "Status"       VARCHAR(50) NOT NULL DEFAULT 'Pending',
    "UpdatedAt"    TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- TABLE: OrderItems
-- ============================================================
CREATE TABLE "OrderItems" (
    "OrderItemId"  SERIAL PRIMARY KEY,
    "OrderId"      INT NOT NULL REFERENCES "Orders"("OrderId") ON DELETE CASCADE,
    "ProductId"    INT NOT NULL REFERENCES "Products"("ProductId"),
    "Quantity"     INT NOT NULL DEFAULT 1,
    "UnitPrice"    NUMERIC(10,2) NOT NULL,
    "UpdatedAt"    TIMESTAMP DEFAULT NOW()
);

-- ============================================================
-- DATA: Roles (3 rows)
-- ============================================================
INSERT INTO "Roles" ("Name", "UpdatedAt") VALUES
('Admin',    NOW()),
('Manager',  NOW()),
('Customer', NOW());

-- ============================================================
-- DATA: Users (10 rows)
-- Password stored as plain text for demo (admin/admin123, etc.)
-- ============================================================
INSERT INTO "Users" ("Login", "Password", "FullName", "Email", "RoleId", "UpdatedAt") VALUES
('admin',       'admin123',     'Mathieu Admin',        'admin@mathieushop.com',        1, NOW()),
('manager1',    'manager123',   'Alice Johnson',        'alice@mathieushop.com',        2, NOW()),
('manager2',    'manager123',   'Bob Martinez',         'bob@mathieushop.com',          2, NOW()),
('customer1',   'pass123',      'Liam Smith',           'liam.smith@email.com',         3, NOW()),
('customer2',   'pass123',      'Emma Jones',           'emma.jones@email.com',         3, NOW()),
('customer3',   'pass123',      'Noah Williams',        'noah.williams@email.com',      3, NOW()),
('customer4',   'pass123',      'Olivia Brown',         'olivia.brown@email.com',       3, NOW()),
('customer5',   'pass123',      'William Davis',        'william.davis@email.com',      3, NOW()),
('customer6',   'pass123',      'Ava Miller',           'ava.miller@email.com',         3, NOW()),
('customer7',   'pass123',      'James Wilson',         'james.wilson@email.com',       3, NOW());

-- ============================================================
-- DATA: Categories (2 rows — Кастом и Косплей)
-- ============================================================
INSERT INTO "Categories" ("Name", "Description", "UpdatedAt") VALUES
('Кастом',   'Кастомные изделия ручной работы',   NOW()),
('Косплей',  'Косплей костюмы и аксессуары',       NOW());

-- ============================================================
-- DATA: Suppliers (10 rows)
-- ============================================================
INSERT INTO "Suppliers" ("CompanyName", "ContactName", "Phone", "Email", "UpdatedAt") VALUES
('ArtCraft Studio',     'Elena Petrova',    '+7-900-001-01-01', 'elena@artcraft.ru',    NOW()),
('CosWorld Ltd',        'Dmitry Ivanov',    '+7-900-001-01-02', 'dmitry@cosworld.ru',   NOW()),
('HandMade Pro',        'Maria Sidorova',   '+7-900-001-01-03', 'maria@handmade.ru',    NOW()),
('FabricMaster',        'Alexey Kozlov',    '+7-900-001-01-04', 'alexey@fabric.ru',     NOW()),
('CosplayHub',          'Natalia Volkova',  '+7-900-001-01-05', 'natalia@coshub.ru',    NOW()),
('CustomKing',          'Igor Sokolov',     '+7-900-001-01-06', 'igor@customking.ru',   NOW()),
('PropFactory',         'Olga Novikova',    '+7-900-001-01-07', 'olga@propfactory.ru',  NOW()),
('SewingAtelier',       'Pavel Morozov',    '+7-900-001-01-08', 'pavel@atelier.ru',     NOW()),
('AnimeCraft',          'Yulia Fedorova',   '+7-900-001-01-09', 'yulia@animecraft.ru',  NOW()),
('MasterPiece Co',      'Sergey Lebedev',   '+7-900-001-01-10', 'sergey@masterpiece.ru',NOW());

-- ============================================================
-- DATA: Products (10 rows — 5 Кастом + 5 Косплей)
-- ImagePath хранит относительный путь к файлу в папке Ресурсы
-- ============================================================
INSERT INTO "Products" ("Name", "Price", "Stock", "ImagePath", "Collection", "CategoryId", "SupplierId", "UpdatedAt") VALUES
('Кастомные кроссовки Nike',        3500.00, 15, 'Ресурсы/pr/Кастом/Pr1.jpg',    'Киберпанк',  1, 1, NOW()),
('Кастомная куртка с принтом',      4200.00, 10, 'Ресурсы/pr/Кастом/Pr2.jpg',    'Нуар',       1, 6, NOW()),
('Кастомная сумка ручной работы',   2800.00, 20, 'Ресурсы/pr/Кастом/Pr3.jpg',    'Аниме',      1, 3, NOW()),
('Кастомная футболка аниме',        1500.00, 30, 'Ресурсы/pr/Кастом/Pr4.jpg',    'Аниме',      1, 4, NOW()),
('Кастомный рюкзак с вышивкой',     3200.00, 12, 'Ресурсы/pr/Кастом/Pr5.jpg',    'Новый год',  1, 8, NOW()),
('Косплей костюм Наруто',           5500.00,  8, 'Ресурсы/pr/Косплей/KL1.jpg',   'Аниме',      2, 5, NOW()),
('Косплей костюм Сейлор Мун',       6200.00,  6, 'Ресурсы/pr/Косплей/KL2.jpg',   'Аниме',      2, 9, NOW()),
('Косплей меч Клинок рассекающий',  2900.00, 14, 'Ресурсы/pr/Косплей/KL3.jpg',   'Аниме',      2, 7, NOW()),
('Косплей маска Тоторо',            1800.00, 25, 'Ресурсы/pr/Косплей/KL4.jpg',   'Хэллоуин',   2, 2, NOW()),
('Косплей парик Микаса',            2100.00, 18, 'Ресурсы/pr/Косплей/KL5.jpg',   'Аниме',      2,10, NOW());

-- ============================================================
-- DATA: Orders (10 rows — привязаны к Users)
-- ============================================================
INSERT INTO "Orders" ("UserId", "OrderDate", "Status", "UpdatedAt") VALUES
(4,  '2025-01-05 10:00:00', 'Delivered',  NOW()),
(5,  '2025-01-12 11:30:00', 'Delivered',  NOW()),
(6,  '2025-02-03 09:15:00', 'Shipped',    NOW()),
(7,  '2025-02-18 14:00:00', 'Shipped',    NOW()),
(8,  '2025-03-07 16:45:00', 'Processing', NOW()),
(9,  '2025-03-20 08:30:00', 'Processing', NOW()),
(10, '2025-04-01 12:00:00', 'Pending',    NOW()),
(4,  '2025-04-10 13:20:00', 'Pending',    NOW()),
(5,  '2025-04-14 17:00:00', 'Cancelled',  NOW()),
(6,  '2025-04-17 10:10:00', 'Pending',    NOW());

-- ============================================================
-- DATA: OrderItems (10 rows)
-- ============================================================
INSERT INTO "OrderItems" ("OrderId", "ProductId", "Quantity", "UnitPrice", "UpdatedAt") VALUES
(1,  1, 1, 3500.00, NOW()),
(2,  6, 1, 5500.00, NOW()),
(3,  2, 1, 4200.00, NOW()),
(4,  7, 1, 6200.00, NOW()),
(5,  3, 2, 2800.00, NOW()),
(6,  8, 1, 2900.00, NOW()),
(7,  4, 3, 1500.00, NOW()),
(8,  9, 2, 1800.00, NOW()),
(9,  5, 1, 3200.00, NOW()),
(10,10, 1, 2100.00, NOW());
