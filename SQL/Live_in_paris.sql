DROP DATABASE IF EXISTS restaurant;
CREATE DATABASE IF NOT EXISTS restaurant;
USE restaurant;

CREATE TABLE IF NOT EXISTS Entreprise (
    no_siret     BIGINT,
    nom          VARCHAR(50) NOT NULL,
    nom_referent VARCHAR(50),
    email        VARCHAR(100),
    code_postal  VARCHAR(50),
    ville        VARCHAR(50),
    PRIMARY KEY (no_siret)
);

CREATE TABLE IF NOT EXISTS Particulier (
    id_particulier        INT,
    tel                   VARCHAR(20),
    code_postal           VARCHAR(50),
    nom                   VARCHAR(60) NOT NULL,
    rue                   VARCHAR(100),
    prenom                VARCHAR(60) NOT NULL,
    email                 VARCHAR(100) NOT NULL,
    numero                VARCHAR(50) NOT NULL,
    ville                 VARCHAR(50) NOT NULL,
    metro_le_plus_proche  VARCHAR(50) NOT NULL,
    PRIMARY KEY (id_particulier)
);

CREATE TABLE IF NOT EXISTS Cuisinier (
    id_cuisinier  INT,
    mot_de_passe_ VARCHAR(75) NOT NULL,
    id_particulier INT NOT NULL,
    PRIMARY KEY (id_cuisinier),
    UNIQUE (id_particulier),
    FOREIGN KEY (id_particulier) REFERENCES Particulier(id_particulier) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Plat (
    id_plat              INT,
    nombre_de_personnes_ BIGINT NOT NULL,
    date_fabric         DATETIME NOT NULL,
    date_peremption     DATETIME,
    quantite            INT NOT NULL,
    description         VARCHAR(300),
    categorie          VARCHAR(50),
    prix              DECIMAL(10,2) NOT NULL,
    regime            VARCHAR(50),
    nationalite_plat  VARCHAR(50),
    nom               VARCHAR(50) NOT NULL,
    nature            VARCHAR(50),
    PRIMARY KEY (id_plat)
);

CREATE TABLE IF NOT EXISTS Ingredient (
    nom        VARCHAR(100),
    allergenes VARCHAR(50),
    volume    VARCHAR(50) NOT NULL,
    PRIMARY KEY (nom)
);

CREATE TABLE IF NOT EXISTS Client (
    id_client     INT,
    mot_de_passe  VARCHAR(255) NOT NULL,
    id_particulier INT NOT NULL,
    no_siret      BIGINT NOT NULL,
    PRIMARY KEY (id_client),
    UNIQUE (id_particulier),
    UNIQUE (no_siret),
    FOREIGN KEY (id_particulier) REFERENCES Particulier(id_particulier) ON DELETE CASCADE,
    FOREIGN KEY (no_siret) REFERENCES Entreprise(no_siret) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS Commande (
    numero_commande        INT,
    adresse_livraison_     VARCHAR(257) NOT NULL,
    prix_ht                DECIMAL(10,2) NOT NULL,
    date_commande          DATETIME NOT NULL,
    instructions_commande  VARCHAR(200),
    prix_livraison         DECIMAL(10,2),
    prix_ttc              DECIMAL(10,2),
    id_client              INT NOT NULL,
    PRIMARY KEY (numero_commande), 
    FOREIGN KEY (id_client) REFERENCES Client(id_client) ON DELETE CASCADE
);


CREATE TABLE IF NOT EXISTS est_constitue_de_ (
    numero_commande INT,
    id_plat        INT,
    quantite       INT,
    PRIMARY KEY (numero_commande, id_plat),
    FOREIGN KEY (numero_commande) REFERENCES Commande(numero_commande) ON DELETE CASCADE,
    FOREIGN KEY (id_plat) REFERENCES Plat(id_plat)
);

CREATE TABLE IF NOT EXISTS est_compose_de_ (
    id_plat INT,
    nom     VARCHAR(100),
    quantite INT,
    PRIMARY KEY (id_plat, nom),
    FOREIGN KEY (id_plat) REFERENCES Plat(id_plat),
    FOREIGN KEY (nom) REFERENCES Ingredient(nom)
);

INSERT INTO Entreprise VALUES (12345678900011, 'La Bonne Boite', 'Jean Martin', 'contact@bonboite.fr', '75011', 'Paris');


INSERT INTO Particulier VALUES 
(1, 'Dupond', 'Marie', 'Rue de la République', '30', '75011','Mdupond@gmail.com', '1234567890', 'Paris' , 'République'),
(2, 'Durand', 'Medhy', 'Rue Cardinet', '15', '75017', 'Mdurand@gmail.com', '1234567890', 'Paris', 'Cardinet');


INSERT INTO Cuisinier VALUES (1, 'mdpCuisinier1', 1);


INSERT INTO Client VALUES (1, 'mdpClient1', 2, 12345678900011);


INSERT INTO Commande VALUES 
(1, '30 Rue de la République, 75011 Paris', 10.00, '2025-01-10', 'Aucune instruction', NULL, NULL, 1),
(2, '30 Rue de la République, 75011 Paris', 5.00, '2025-01-10', 'Livrer le matin', NULL, NULL, 1);

INSERT INTO Plat VALUES 
(1, 6, '2025-01-10', '2025-01-15', 6, 'Plat', 'Plat principal', 10.00, '', 'Française', 'Raclette', 'Française'),
(2, 6, '2025-01-10', '2025-01-15', 6, 'Dessert', 'Dessert', 5.00, 'Végétarien', 'Indifférent', 'Salade de fruit', 'Indifférent');

INSERT INTO est_constitue_de_ VALUES (1, 1, 6), (2, 2, 6);

INSERT INTO Ingredient VALUES 
('raclette fromage', '', '250g'),
('pommes_de_terre', '', '200g'),
('jambon', '', '200g'),
('cornichon', '', '3p'),
('fraise', '', '100g'),
('kiwi', '', '100g'),
('sucre', '', '10g');

INSERT INTO est_compose_de_ VALUES 
(1, 'raclette fromage', 250),
(1, 'pommes_de_terre', 200),
(1, 'jambon', 200),
(1, 'cornichon', 3),
(2, 'fraise', 100),
(2, 'kiwi', 100),
(2, 'sucre', 10);

-- Quelques requêtes simples sur votre base de données ainsi créée

UPDATE Particulier SET email = 'nouvel_email@gmail.com' WHERE id_particulier = 1;

UPDATE Plat SET prix = prix + 2.00 WHERE id_plat = 1;

-- SELECT * FROM Plat ORDER BY prix ASC;

ALTER TABLE Particulier
ADD plat_deja_commande VARCHAR(255) NOT NULL;

UPDATE Particulier
SET plat_deja_commande = 'Raclette'
WHERE id_particulier = 1;

SELECT id_particulier, nom, prenom, plat_deja_commande
FROM Particulier;