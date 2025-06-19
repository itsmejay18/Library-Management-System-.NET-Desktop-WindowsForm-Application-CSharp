-- MySQL dump 10.13  Distrib 8.0.40, for Win64 (x86_64)
--
-- Host: localhost    Database: librarydb
-- ------------------------------------------------------
-- Server version	8.0.40

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `authors_tbl`
--

DROP TABLE IF EXISTS `authors_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `authors_tbl` (
  `author_id` int NOT NULL AUTO_INCREMENT,
  `author_name` varchar(45) NOT NULL,
  `contact_info` varchar(45) DEFAULT NULL,
  `biography` text,
  `author_photo_path` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`author_id`)
) ENGINE=InnoDB AUTO_INCREMENT=97 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `authors_tbl`
--

LOCK TABLES `authors_tbl` WRITE;
/*!40000 ALTER TABLE `authors_tbl` DISABLE KEYS */;
INSERT INTO `authors_tbl` VALUES (1,'Thomas H. Cormen','cormen@example.com','Co-author of \"Introduction to Algorithms\" and a prominent figure in computer science.',NULL),(2,'Harper Lee','harper.lee@example.com','Author of the Pulitzer Prize-winning novel \"To Kill a Mockingbird\".',NULL),(3,'Robert C. Martin','unclebob@example.com','Known as \"Uncle Bob\", he is the author of \"Clean Code\" and a leader in Agile practices.',NULL),(4,'George Orwell','george.orwell@example.com','British author famous for dystopian novels like \"1984\" and \"Animal Farm\".',NULL),(5,'Erich Gamma','gamma@example.com','One of the Gang of Four, co-author of \"Design Patterns\".',NULL),(6,'Donald Knuth','knuth@example.com','Renowned computer scientist and author of \"The Art of Computer Programming\".',NULL),(7,'J.K. Rowling','jk.rowling@example.com','British author of the bestselling \"Harry Potter\" series.',NULL),(8,'Douglas Schmidt','doug.schmidt@example.com','Expert in software engineering and middleware frameworks.',NULL),(9,'Kentarou Miura','091658113','Japanese Mangaka, Famous for BERSERK',NULL),(10,'Herman Melville','herman@gmail.com','American novelist',NULL),(11,'Tatsuki Fujimoto','tatsuki@gmail.com','Japanese manga artist, known for his works Fire Punch and Chainsaw Man.',NULL),(12,'One','onetwothereee@gmail.com','Japanese manga artist',NULL),(13,'Harper Lee','harperlee@gmail.com','merican novelist whose 1960 novel To Kill a Mockingbird won the 1961 Pulitzer Prize and became a classic of modern American literature',NULL),(15,'James Arthur','JAmesArthers@gmail.com',' English singer and songwriter. He rose to fame after winning the ninth series of The X Factor in 2012.',NULL),(76,'J.K. Rowling','jkrowling@example.com','Author of the Harry Potter series.',NULL),(77,'George R.R. Martin','grrm@example.com','Author of A Song of Ice and Fire series.',NULL),(78,'Stephen King','sking@example.com','Prolific author of horror and suspense novels.',NULL),(79,'Agatha Christie','achristie@example.com','Famous for her detective novels, especially Hercule Poirot.',NULL),(80,'Dan Brown','danbrown@example.com','Known for The Da Vinci Code.',NULL),(81,'Paulo Coelho','pcoelho@example.com','Brazilian author of The Alchemist.',NULL),(82,'Haruki Murakami','hmurakami@example.com','Renowned for surreal fiction and metaphysical narratives.',NULL),(83,'Isabel Allende','iallende@example.com','Chilean writer famous for magic realism.',NULL),(84,'Neil Gaiman','ngaiman@example.com','Writer of fantasy and comic books like Coraline and Sandman.',NULL),(85,'Margaret Atwood','matwood@example.com','Best known for The Handmaid’s Tale.',NULL),(86,'Rick Riordan','rriordan@example.com','Known for the Percy Jackson series.',NULL),(87,'Suzanne Collins','scollins@example.com','Author of The Hunger Games trilogy.',NULL),(88,'John Green','jgreen@example.com','Author of The Fault in Our Stars.',NULL),(89,'Khaled Hosseini','khosseini@example.com','Afghan-American author of The Kite Runner.',''),(90,'E.L. James','eljames@example.com','Best known for the Fifty Shades trilogy.',NULL),(91,'James Patterson','jpatterson@example.com','Popular thriller writer.',''),(92,'Toni Morrison','tmorrison@example.com','Pulitzer Prize-winning author of Beloved.',''),(93,'Chimamanda Ngozi Adichie','cadichie@example.com','Nigerian writer of Half of a Yellow Sun.',''),(94,'Brandon Sanderson','bsanderson@example.com','Fantasy author of Mistborn and Stormlight Archive.',''),(95,'Colleen Hoover','choover@example.com','Author of contemporary romance and fiction.',''),(96,'Juan Dela Plus','juandeleplus@gmail.com','filipino Artist',NULL);
/*!40000 ALTER TABLE `authors_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `books_tbl`
--

DROP TABLE IF EXISTS `books_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `books_tbl` (
  `book_id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(45) NOT NULL,
  `category` varchar(45) NOT NULL,
  `genre` varchar(45) NOT NULL,
  `ISBN` varchar(20) DEFAULT NULL,
  `author_id` int DEFAULT NULL,
  `publisher_id` int DEFAULT NULL,
  `year_published` int DEFAULT NULL,
  `book_status` enum('Available','Borrowed') DEFAULT 'Available',
  `description` text,
  `book_cover_path` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`book_id`),
  UNIQUE KEY `ISBN_UNIQUE` (`ISBN`),
  KEY `FK_books_author_id_idx` (`author_id`),
  KEY `FK_book_author_id_idx` (`author_id`),
  KEY `FK_books_publisher_id` (`publisher_id`),
  CONSTRAINT `FK_books_author_id` FOREIGN KEY (`author_id`) REFERENCES `authors_tbl` (`author_id`),
  CONSTRAINT `FK_books_publisher_id` FOREIGN KEY (`publisher_id`) REFERENCES `publishers_tbl` (`publisher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=211 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `books_tbl`
--

LOCK TABLES `books_tbl` WRITE;
/*!40000 ALTER TABLE `books_tbl` DISABLE KEYS */;
INSERT INTO `books_tbl` VALUES (9,'Berserk','Fiction','Fantasy','15123171341',9,5,2002,'Borrowed',NULL,NULL),(10,'Moby Dick','Non-Fiction','Historical Fiction','151212',10,6,1965,'Available','Man sees big fish, big fish bit Man',NULL),(11,'Chainsaw Man','Fiction','Sci-Fi','14124',11,5,2016,'Available','',NULL),(12,'One Punch Man','Fiction','Sci-Fi','1908501qjgv9-0uc',12,5,2024,'Available','One-Punch Man (Japanese: ワンパンマン, Hepburn: Wanpan Man, stylized OnePunch-Man) is a Japanese superhero manga series created by One. It tells the story of Saitama, an independent superhero who, because he can defeat any opponent with a single punch due to having trained himself to his peak condition',NULL),(14,'Science','Non-Fiction','Science','12345',15,8,2025,'Available','',NULL),(209,'Intro to Algorithms','Non-Fiction','Science','9780262033848',96,29,2006,'Available','',NULL);
/*!40000 ALTER TABLE `books_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `borrowers_tbl`
--

DROP TABLE IF EXISTS `borrowers_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `borrowers_tbl` (
  `borrower_id` int NOT NULL AUTO_INCREMENT,
  `book_id` int NOT NULL,
  `member_id` int NOT NULL,
  `borrow_date` date NOT NULL,
  `due_date` date NOT NULL,
  `return_date` date DEFAULT NULL,
  PRIMARY KEY (`borrower_id`),
  KEY `FK_book_id_idx` (`book_id`),
  KEY `FK_member_id_idx` (`member_id`),
  CONSTRAINT `FK_borrowers_book_id` FOREIGN KEY (`book_id`) REFERENCES `books_tbl` (`book_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_borrowers_member_id` FOREIGN KEY (`member_id`) REFERENCES `members_tbl` (`member_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `borrowers_tbl`
--

LOCK TABLES `borrowers_tbl` WRITE;
/*!40000 ALTER TABLE `borrowers_tbl` DISABLE KEYS */;
INSERT INTO `borrowers_tbl` VALUES (30,9,23,'2024-12-09','2024-12-16','2024-12-09'),(31,9,23,'2024-12-09','2024-12-16','2024-12-09'),(32,9,23,'2024-12-09','2024-12-16','2024-12-09'),(33,10,23,'2024-12-09','2024-12-16','2024-12-14'),(34,9,23,'2024-12-16','2024-12-23','2024-12-18'),(35,12,23,'2024-12-18','2024-12-25','2025-02-22'),(36,9,23,'2024-12-18','2024-12-25','2025-02-13'),(37,14,26,'2025-02-13','2025-02-20','2025-02-22'),(38,9,23,'2025-02-22','2025-03-01','2025-02-27'),(39,10,23,'2025-02-27','2025-03-06','2025-02-27'),(40,12,23,'2025-02-27','2025-03-06','2025-02-27'),(41,9,40,'2025-05-06','2025-05-13',NULL),(42,9,38,'2025-06-08','2025-06-15',NULL),(43,9,23,'2025-06-08','2025-06-15','2025-06-08'),(44,14,24,'2025-06-08','2025-06-15','2025-06-08'),(45,9,28,'2025-06-08','2025-06-15','2025-06-08'),(46,9,28,'2025-06-08','2025-06-15','2025-06-09'),(47,10,23,'2025-06-09','2025-06-16','2025-06-09'),(48,209,28,'2025-06-09','2025-06-16','2025-06-09'),(49,12,28,'2025-06-09','2025-06-16','2025-06-09'),(50,9,28,'2025-06-09','2025-06-16',NULL);
/*!40000 ALTER TABLE `borrowers_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dues_tbl`
--

DROP TABLE IF EXISTS `dues_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dues_tbl` (
  `due_id` int NOT NULL AUTO_INCREMENT,
  `borrow_id` int DEFAULT NULL,
  `fine_amount` int DEFAULT NULL,
  `dues_status` enum('Paid','Unpaid') DEFAULT NULL,
  PRIMARY KEY (`due_id`),
  KEY `FK_dues_borrow_id_idx` (`borrow_id`),
  CONSTRAINT `FK_dues_borrower_id` FOREIGN KEY (`borrow_id`) REFERENCES `borrowers_tbl` (`borrower_id`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dues_tbl`
--

LOCK TABLES `dues_tbl` WRITE;
/*!40000 ALTER TABLE `dues_tbl` DISABLE KEYS */;
INSERT INTO `dues_tbl` VALUES (10,30,0,'Paid'),(11,31,0,'Paid'),(12,32,0,'Paid'),(13,33,0,'Paid'),(14,34,0,'Paid'),(15,35,295,'Paid'),(16,36,250,'Paid'),(17,37,10,'Paid'),(18,38,0,'Paid'),(19,39,0,'Paid'),(20,40,0,'Paid'),(21,41,0,'Unpaid'),(22,42,0,'Unpaid'),(23,43,0,'Paid'),(24,44,0,'Paid'),(25,45,0,'Paid'),(26,46,0,'Paid'),(27,47,0,'Paid'),(28,48,0,'Paid'),(29,49,0,'Paid'),(30,50,0,'Unpaid');
/*!40000 ALTER TABLE `dues_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `interlibrary_loans`
--

DROP TABLE IF EXISTS `interlibrary_loans`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `interlibrary_loans` (
  `ill_id` int NOT NULL,
  `material_title` varchar(255) DEFAULT NULL,
  `material_type` enum('Book','Dissertation','Article/Journal') DEFAULT NULL,
  `request_date` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`ill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `interlibrary_loans`
--

LOCK TABLES `interlibrary_loans` WRITE;
/*!40000 ALTER TABLE `interlibrary_loans` DISABLE KEYS */;
INSERT INTO `interlibrary_loans` VALUES (1,'Epic Reads Book Club Sampler','Book','2025-06-09'),(2,'Twinkle2 Little Star','Article/Journal','2025-06-09'),(3,'tabang','Dissertation','2025-06-09');
/*!40000 ALTER TABLE `interlibrary_loans` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `members_tbl`
--

DROP TABLE IF EXISTS `members_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `members_tbl` (
  `member_id` int NOT NULL AUTO_INCREMENT,
  `first_name` varchar(45) DEFAULT NULL,
  `last_name` varchar(45) DEFAULT NULL,
  `email` varchar(45) DEFAULT NULL,
  `phone` varchar(45) DEFAULT NULL,
  `member_type` enum('Teacher','Staff','Student') DEFAULT NULL,
  `membership_date` datetime DEFAULT CURRENT_TIMESTAMP,
  `member_status` enum('Active','Inactive') DEFAULT 'Active',
  `profile_pic` varchar(225) DEFAULT NULL,
  PRIMARY KEY (`member_id`)
) ENGINE=InnoDB AUTO_INCREMENT=68 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `members_tbl`
--

LOCK TABLES `members_tbl` WRITE;
/*!40000 ALTER TABLE `members_tbl` DISABLE KEYS */;
INSERT INTO `members_tbl` VALUES (23,'Jericho Earl','Martin','jerichomartin805@gmail.com','09111111111111','Student','2024-12-09 10:46:04','Active',NULL),(24,'Jay','Ababon','itsmeJay21@gmail.com','09912268122','Student','2024-12-14 08:25:12','Active',NULL),(26,'Jeryza','ortega','ortega@gmail.com','123456','Student','2025-02-13 13:14:28','Active',NULL),(28,'Irah Nicca','Tampos','irah@gmail.com','09343342556545','Student','2025-02-22 17:24:57','Active',NULL),(29,'Justine','Aguirre','justinaguirre@gmail.com','094543543544','Student','2025-02-22 21:27:01','Active',NULL),(30,'Liezl','Albarina','Albarina,Liezl Montecillo@gmail.com','097866775657','Student','2025-02-22 21:29:20','Active',NULL),(31,'Sheryl','Aleviado','Aleviado,Sheryl@gmail.com','0978678686768','Student','2025-02-22 21:30:04','Active',NULL),(32,'Gabriel','Apostol','Apostol,Gabriel C.@gmail.com','0943534556546','Student','2025-02-22 21:30:43','Active',NULL),(33,'Jeamart','Babao','Babao,Jeamart A.@gmail.com','094546546467','Student','2025-02-22 21:31:30','Active',NULL),(34,'Bryan Jhon','Baritugo','Baritugo,BryanJhonV.@gmail.com','0943545435435','Student','2025-02-22 21:32:20','Active',NULL),(35,'Jepherson','Broñosa','Broñosa,Jepherson@gmail.com','094323453453','Student','2025-02-22 21:33:58','Active',NULL),(36,'Jasper Leen','Bunday','Bunday,JasperLeenC.@gmail.com','0954375475455','Student','2025-02-22 21:35:37','Active',NULL),(37,'Wilfred','Daligdig','Daligdig,WilfredJr.@gmail.com','0934343843444','Student','2025-02-22 21:36:24','Active',NULL),(38,'Zanny-Dream','Dalumpines','Dalumpines,Zanny-Dream@gmail.com','093473443433','Student','2025-02-22 21:37:01','Active',NULL),(39,'Cyral','Dalen','Dalen,Cyral@gmail.com','0932939239','Student','2025-02-22 21:37:39','Active',NULL),(40,'Christian','Langote','DyneLangote,Christian@gmail.com','092343432444','Student','2025-02-22 21:38:17','Active',NULL),(41,'Rubyann','Eduria','Eduria,RubyannT.@gmail.com','094335464444','Student','2025-02-22 21:38:55','Active',NULL),(42,'Laimar','Escalante','Escalante,Laimar@gmail.com','095454545444','Student','2025-02-22 21:39:38','Active',NULL),(43,'Danilene','Fajerga','Fajerga,Danilene@gmail.com','0923283823233','Student','2025-02-22 21:40:18','Active',NULL),(44,'Jir Aiza','Jani','Jani,JirAiza@gmail.com','0932323232333','Student','2025-02-22 21:41:03','Active',NULL),(45,'Ivann','Jumawan','Jumawan, Ivann','09348387437843','Student','2025-02-22 21:41:37','Active',NULL),(46,'Mark Liones','Juntilla','Juntilla,MarkLiones@gmail.com','0934448545434','Student','2025-02-22 21:42:24','Active',NULL),(47,'Mike Eury','Mike Eury','Laugo,MikeEuryPicate@gmail.com','093434384444','Student','2025-02-22 21:43:03','Active',NULL),(48,'Erica','Erica','Manguilimotan,Erica@gmail.com','09843743343433','Student','2025-02-22 21:43:37','Active',NULL),(49,'Zoeh Emery','Manalili','Manalili, ZoehEmery@gmail.com','0945735445455','Student','2025-02-22 21:44:19','Active',NULL),(50,'Mark Jovin','Molina','Molina,MarkJovin@gmail.com','0933284923433','Student','2025-02-22 21:44:55','Active',NULL),(51,'Kien Cyrl','Naran','Naran,KienCyrlPejo@gmail.com','094534854975','Student','2025-02-22 21:45:40','Active',NULL),(52,'Zeus Immanuel','Nuevo','Nuevo,ZeusImmanuelA.@gmail.com','0932942843434','Student','2025-02-22 21:46:25','Active',NULL),(53,'Mae Ann','Olitres','Olitres, Mae Ann','093243284344','Student','2025-02-22 21:46:53','Active',NULL),(54,'Red Angelo','Panugaling','Panugaling, Red Angelo','093423443434342','Student','2025-02-22 21:47:20','Active',NULL),(55,'Hazel','Sabando','Sabando,Hazel@gmail.com','093423432436','Student','2025-02-22 21:47:50','Active',NULL),(56,'Zaldy','Sacedor','Sacedor,Zaldy@gmail.com','092348324734','Student','2025-02-22 21:48:22','Active',NULL),(57,'Ivan','Saragena','Saragena,Ivan@gmail.com','093473643843','Student','2025-02-22 21:49:03','Active',NULL),(58,'John Russel','Sarita','Sarita,JohnRusselL.@gmail.com','09932423844','Student','2025-02-22 21:49:46','Active',NULL),(59,'Mark','Sarabillo','Sarabillo,Mark@gmail.com','0923247343434','Student','2025-02-22 21:50:59','Active',NULL),(60,'John Kenneth','Susada,','Susada,JohnKenneth@gmail.com','093248343444','Student','2025-02-22 21:51:45','Active',NULL),(61,'Reynaldo','Torres','Torres,ReynaldoB.@gmail.com','0945843854545','Student','2025-02-22 21:52:23','Active',NULL),(62,'Daryll','Velonio','Velonio,Daryll@gmail.com','0984328432432','Student','2025-02-22 21:52:59','Active',NULL),(63,'Kian','Villaluz','Villaluz,Kian@gmail.com','0934734423333','Student','2025-02-22 21:53:35','Active',NULL),(65,'Barbie','Fruta','fruta@gmail.com','0937462372323','Student','2025-02-26 21:29:21','Active',NULL),(66,'Jhon Mathew','Eticano','jhon@gmail.com','09234723423','Student','2025-02-26 21:32:43','Active',NULL),(67,'Kate','Dela Cruz','katedelacruz@gmail.com','09123456789','Student','2025-02-27 13:13:45','Active',NULL);
/*!40000 ALTER TABLE `members_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `publishers_tbl`
--

DROP TABLE IF EXISTS `publishers_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `publishers_tbl` (
  `publisher_id` int NOT NULL AUTO_INCREMENT,
  `publisher_name` varchar(45) NOT NULL,
  `contact_info` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`publisher_id`)
) ENGINE=InnoDB AUTO_INCREMENT=30 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `publishers_tbl`
--

LOCK TABLES `publishers_tbl` WRITE;
/*!40000 ALTER TABLE `publishers_tbl` DISABLE KEYS */;
INSERT INTO `publishers_tbl` VALUES (5,'Shonen Jump','121431'),(6,'Harper And Brother','Harper&brother@gmail.com'),(7,'J. B. Lippincott & Co.','lippoopincolt@gmail.com'),(8,'cecelib','celilib@gmail.com'),(9,'Penguin Random House','contact@penguinrandomhouse.com'),(10,'HarperCollins','info@harpercollins.com'),(11,'Simon & Schuster','hello@simonandschuster.com'),(12,'Hachette Book Group','support@hachettebookgroup.com'),(13,'Macmillan Publishers','contact@macmillan.com'),(14,'Scholastic Inc.','info@scholastic.com'),(15,'Oxford University Press','admin@oup.com'),(16,'Cambridge University Press','connect@cambridge.org'),(17,'Bloomsbury Publishing','info@bloomsbury.com'),(18,'Tor Books','support@tor.com'),(19,'Candlewick Press','press@candlewick.com'),(20,'Vintage Books','feedback@vintagebooks.com'),(21,'Faber & Faber','editorial@faber.co.uk'),(22,'Little, Brown and Company','info@littlebrown.com'),(23,'Avon Books','help@avonbooks.com'),(24,'Anchor Books','anchor@randomhouse.com'),(25,'Crown Publishing Group','crown@penguin.com'),(26,'Del Rey Books','contact@delrey.com'),(27,'Orbit Books','orbit@hbgusa.com'),(28,'Zondervan','contact@zondervan.com'),(29,'Ruvina Corporation','RuvinaCorporation@gmail.com');
/*!40000 ALTER TABLE `publishers_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `session_log`
--

DROP TABLE IF EXISTS `session_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `session_log` (
  `log_id` int NOT NULL AUTO_INCREMENT,
  `user_id` int DEFAULT NULL,
  `activity` varchar(255) DEFAULT NULL,
  `activity_date` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`log_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `session_log`
--

LOCK TABLES `session_log` WRITE;
/*!40000 ALTER TABLE `session_log` DISABLE KEYS */;
/*!40000 ALTER TABLE `session_log` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transactions_tbl`
--

DROP TABLE IF EXISTS `transactions_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transactions_tbl` (
  `transaction_id` int NOT NULL AUTO_INCREMENT,
  `borrower_id` int NOT NULL,
  `transaction_date` datetime DEFAULT NULL,
  `transaction_status` enum('Borrowed','Returned','Overdue') DEFAULT 'Borrowed',
  `fine` decimal(10,2) DEFAULT NULL,
  PRIMARY KEY (`transaction_id`),
  KEY `FK_transaction_borrower_id_idx` (`borrower_id`),
  CONSTRAINT `FK_transaction_borrower_id` FOREIGN KEY (`borrower_id`) REFERENCES `borrowers_tbl` (`borrower_id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=36 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transactions_tbl`
--

LOCK TABLES `transactions_tbl` WRITE;
/*!40000 ALTER TABLE `transactions_tbl` DISABLE KEYS */;
INSERT INTO `transactions_tbl` VALUES (29,47,'2025-06-09 00:02:15','Borrowed',0.00),(30,47,'2025-06-09 00:09:22','Returned',0.00),(31,46,'2025-06-09 00:09:43','Returned',0.00),(32,48,'2025-06-09 00:24:20','Returned',0.00),(33,49,'2025-06-09 00:25:04','Borrowed',0.00),(34,49,'2025-06-09 00:25:48','Returned',0.00),(35,50,'2025-06-09 08:43:15','Borrowed',0.00);
/*!40000 ALTER TABLE `transactions_tbl` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users_tbl`
--

DROP TABLE IF EXISTS `users_tbl`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users_tbl` (
  `user_id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(45) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` enum('Admin','Staff') NOT NULL,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users_tbl`
--

LOCK TABLES `users_tbl` WRITE;
/*!40000 ALTER TABLE `users_tbl` DISABLE KEYS */;
INSERT INTO `users_tbl` VALUES (1,'admin','d74ff0ee8da3b9806b18c877dbf29bbde50b5bd8e4dad7a3a725000feb82e8f1','Admin'),(2,'staff','d74ff0ee8da3b9806b18c877dbf29bbde50b5bd8e4dad7a3a725000feb82e8f1','Staff'),(3,'jay','bfef4adc39f01b033fe749bb5f28f10b581fef319d34445d21a7bc63fe732fa3','Admin'),(5,'joy','b113405b6f9114b2abac7676f6868df33b0dde53f2e0ddd0f5340392563ab5f3','Staff'),(6,'irah','eb7ef44dec56c7b804888d18787aeb390d4fc1df303f57ae022bea14d0cec0c5','Staff'),(7,'jeryza','fa0546dec34e9d270e68e12962ae04069b736ec6eb2541cd0d1a04545aa45398','Admin');
/*!40000 ALTER TABLE `users_tbl` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-19  9:26:53
