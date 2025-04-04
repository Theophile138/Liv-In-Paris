DROP DATABASE IF EXISTS restaurant;

CREATE DATABASE IF NOT EXISTS restaurant;
USE restaurant;


DROP TABLE IF EXISTS Particulier;
CREATE TABLE IF NOT EXISTS Particulier (
    id_particulier        INT AUTO_INCREMENT PRIMARY KEY,
    tel                   VARCHAR(20),
    code_postal           VARCHAR(50),
    nom                   VARCHAR(60) NOT NULL,
    rue                   VARCHAR(100),
    prenom                VARCHAR(60) NOT NULL,
    email                 VARCHAR(100) NOT NULL,
    numero                VARCHAR(50) NOT NULL,
    ville                 VARCHAR(50) NOT NULL,
    metro_le_plus_proche  VARCHAR(50) NOT NULL
);


DROP TABLE IF EXISTS Entreprise;
CREATE TABLE IF NOT EXISTS Entreprise (
    no_siret     BIGINT PRIMARY KEY,
    nom          VARCHAR(50) NOT NULL,
    nom_referent VARCHAR(50),
    email        VARCHAR(100),
    code_postal  VARCHAR(50),
    ville        VARCHAR(50)
);


DROP TABLE IF EXISTS Client;
CREATE TABLE IF NOT EXISTS Client (
    id_client     INT PRIMARY KEY,
    mot_de_passe  VARCHAR(255) NOT NULL,
    id_particulier INT NOT NULL UNIQUE,
    no_siret      BIGINT NOT NULL,
    FOREIGN KEY (id_particulier) REFERENCES Particulier(id_particulier) ON DELETE CASCADE,
    FOREIGN KEY (no_siret) REFERENCES Entreprise(no_siret) ON DELETE CASCADE
);


DROP TABLE IF EXISTS Cuisinier;
CREATE TABLE IF NOT EXISTS Cuisinier (
    id_cuisinier    INT PRIMARY KEY,
    mot_de_passe_   VARCHAR(75) NOT NULL,
    id_particulier  INT NOT NULL UNIQUE,
    FOREIGN KEY (id_particulier) REFERENCES Particulier(id_particulier) ON DELETE CASCADE
);


DROP TABLE IF EXISTS Plat;
CREATE TABLE IF NOT EXISTS Plat (
    id_plat              INT PRIMARY KEY,
    nombre_de_personnes_ BIGINT NOT NULL,
    date_fabric          DATETIME NOT NULL,
    date_peremption      DATETIME,
    quantite             INT NOT NULL,
    description          VARCHAR(300),
    categorie            VARCHAR(50),
    prix                 DECIMAL(10,2) NOT NULL,
    regime               VARCHAR(50),
    nationalite_plat     VARCHAR(50),
    nom                  VARCHAR(50) NOT NULL,
    nature               VARCHAR(50),
    id_cuisinier         INT,
    FOREIGN KEY (id_cuisinier) REFERENCES Cuisinier(id_cuisinier) ON DELETE SET NULL
);


DROP TABLE IF EXISTS Ingredient;
CREATE TABLE IF NOT EXISTS Ingredient (
    nom        VARCHAR(100) PRIMARY KEY,
    allergenes VARCHAR(50),
    volume     VARCHAR(50) NOT NULL
);


DROP TABLE IF EXISTS Commande;
CREATE TABLE IF NOT EXISTS Commande (
    numero_commande         INT AUTO_INCREMENT PRIMARY KEY ,
    adresse_livraison_     VARCHAR(257) NOT NULL,
    prix_ht                DECIMAL(10,2) NOT NULL,
    date_commande          DATETIME NOT NULL,
    instructions_commande  VARCHAR(200),
    prix_livraison         DECIMAL(10,2),
    prix_ttc               DECIMAL(10,2),
    statut                 VARCHAR(50) DEFAULT 'En attente',
    id_client              INT NOT NULL,
    FOREIGN KEY (id_client) REFERENCES Client(id_client) ON DELETE CASCADE
);


DROP TABLE IF EXISTS Commande_Plat;
CREATE TABLE IF NOT EXISTS Commande_Plat (
    numero_commande INT,
    id_plat         INT,
    quantite        INT DEFAULT 1,
    PRIMARY KEY (numero_commande, id_plat),
    FOREIGN KEY (numero_commande) REFERENCES Commande(numero_commande) ON DELETE CASCADE,
    FOREIGN KEY (id_plat) REFERENCES Plat(id_plat) ON DELETE CASCADE
);




INSERT INTO Particulier (tel, code_postal, nom, rue, prenom, email, numero, ville, metro_le_plus_proche) VALUES
('0600000001', '75001', 'Durand', 'Rue A', 'Alice', 'alice@ex.com', '12', 'Paris', 'Louvre'),
('0600000002', '75002', 'Martin', 'Rue B', 'Bob', 'bob@ex.com', '34', 'Paris', 'Bourse'),
('0600000003', '75003', 'Lemoine', 'Rue C', 'Chloé', 'chloe@ex.com', '56', 'Paris', 'Réaumur'),
('0600000004', '75004', 'Petit', 'Rue D', 'David', 'david@ex.com', '78', 'Paris', 'Hôtel de Ville'),
('0600000005', '75005', 'Roux', 'Rue E', 'Emma', 'emma@ex.com', '90', 'Paris', 'Place Monge');


INSERT INTO Entreprise (no_siret, nom, nom_referent, email, code_postal, ville) VALUES
(12345678900011, 'La Bonne Bouffe', 'Chef Jean', 'contact@bonnebouffe.com', '75001', 'Paris'),
(12345678900012, 'Saveurs du Monde', 'Sophie Gourmet', 'sophie@saveurs.com', '75002', 'Paris'),
(12345678900013, 'Traditions Locales', 'Marc Cuisine', 'marc@local.fr', '75003', 'Paris');

INSERT INTO Client (id_client, mot_de_passe, id_particulier, no_siret) VALUES
(101, 'mdp123', 1, 12345678900011),
(102, 'mdp456', 2, 12345678900012),
(103, 'mdp789', 3, 12345678900013);

INSERT INTO Cuisinier (id_cuisinier, mot_de_passe_, id_particulier) VALUES
(201, 'cuisine456', 4),
(202, 'cuisine789', 5);


INSERT INTO Plat (id_plat, nombre_de_personnes_, date_fabric, date_peremption, quantite, description, categorie, prix, regime, nationalite_plat, nom, nature, id_cuisinier) VALUES
(301, 2, NOW(), DATE_ADD(NOW(), INTERVAL 3 DAY), 5, 'Délicieux curry végétarien', 'Plat principal', 14.99, 'Végétarien', 'Indienne', 'Curry Masala', 'Chaud', 201),
(302, 4, NOW(), DATE_ADD(NOW(), INTERVAL 2 DAY), 8, 'Poulet sauce soja', 'Plat principal', 16.50, 'Omnivore', 'Chinoise', 'Poulet Impérial', 'Chaud', 201),
(303, 3, NOW(), DATE_ADD(NOW(), INTERVAL 5 DAY), 10, 'Tacos mexicains épicés', 'Snack', 12.00, 'Omnivore', 'Mexicaine', 'Tacos Locos', 'Chaud', 202),
(304, 1, NOW(), DATE_ADD(NOW(), INTERVAL 2 DAY), 6, 'Soupe miso légère', 'Entrée', 7.50, 'Vegan', 'Japonaise', 'Soupe Miso', 'Chaud', 202),
(305, 2, NOW(), DATE_ADD(NOW(), INTERVAL 4 DAY), 9, 'Paëlla valencienne', 'Plat principal', 18.75, 'Pescetarien', 'Espagnole', 'Paëlla Royale', 'Chaud', 201),
(306, 1, NOW(), DATE_ADD(NOW(), INTERVAL 2 DAY), 5, 'Salade César', 'Entrée', 9.50, 'Omnivore', 'Américaine', 'Salade César', 'Froid', 202);


INSERT INTO Commande (numero_commande, adresse_livraison_, prix_ht, date_commande, instructions_commande, prix_livraison, prix_ttc, statut, id_client) VALUES
(401, '12 Rue A, 75001 Paris', 14.99, NOW(), 'Sans oignon', 2.00, 20.99, 'En attente', 101),
(402, '34 Rue B, 75002 Paris', 16.50, NOW(), 'Ajoutez des baguettes', 2.50, 24.30, 'Livrée', 102),
(403, '56 Rue C, 75003 Paris', 12.00, NOW(), '', 1.50, 16.90, 'En cours', 103),
(404, '78 Rue D, 75004 Paris', 18.75, NOW(), 'Allergie aux fruits à coque', 3.00, 26.70, 'En attente', 101);


INSERT INTO Commande_Plat (numero_commande, id_plat, quantite) VALUES
(401, 301, 1),
(402, 302, 2),
(403, 303, 1),
(404, 305, 1),
(404, 304, 1);


SELECT * FROM Client;
SELECT * FROM Cuisinier;
SELECT * FROM Plat;
SELECT * FROM Commande;

SELECT p.nom, cp.quantite FROM Plat p
JOIN Commande_Plat cp ON p.id_plat = cp.id_plat
WHERE cp.numero_commande = 404;

SELECT COUNT(*) AS total_commandes FROM Commande;
SELECT AVG(prix_ttc) AS moyenne_prix_ttc FROM Commande;
SELECT nationalite_plat, COUNT(*) AS total FROM Plat GROUP BY nationalite_plat;

ALTER TABLE Particulier ADD COLUMN plat_deja_commande VARCHAR(100);

SELECT * FROM Commande_Plat LIMIT 10;
