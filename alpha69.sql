-- --------------------------------------------------------
-- Vært:                         alpha69.cfuzrlbo4jgs.eu-central-1.rds.amazonaws.com
-- Server-version:               5.7.21 - MySQL Community Server (GPL)
-- ServerOS:                     Linux
-- HeidiSQL Version:             9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Dumping database structure for alpha69
DROP DATABASE IF EXISTS `alpha69`;
CREATE DATABASE IF NOT EXISTS `alpha69` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `alpha69`;

-- Dumping structure for tabel alpha69.live_sessions
DROP TABLE IF EXISTS `live_sessions`;
CREATE TABLE IF NOT EXISTS `live_sessions` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `title` varchar(191) NOT NULL,
  `description` varchar(191) DEFAULT NULL,
  `min_user_score` decimal(13,2) unsigned NOT NULL DEFAULT '0.00',
  `tags` varchar(191) DEFAULT NULL,
  `started_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ended_at` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.live_session_models
DROP TABLE IF EXISTS `live_session_models`;
CREATE TABLE IF NOT EXISTS `live_session_models` (
  `livesession_id` int(10) unsigned NOT NULL,
  `model_id` int(10) unsigned DEFAULT NULL,
  `is_host` tinyint(4) NOT NULL DEFAULT '0',
  `joined_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `left_at` datetime DEFAULT NULL,
  KEY `PK_livesession_id` (`livesession_id`,`model_id`),
  KEY `FK_livesession_models_model_id_idx` (`model_id`),
  CONSTRAINT `FK_livesession_id` FOREIGN KEY (`livesession_id`) REFERENCES `live_sessions` (`id`),
  CONSTRAINT `FK_livesession_models_model_id` FOREIGN KEY (`model_id`) REFERENCES `models` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.live_session_viewers
DROP TABLE IF EXISTS `live_session_viewers`;
CREATE TABLE IF NOT EXISTS `live_session_viewers` (
  `livesession_id` int(10) unsigned NOT NULL,
  `user_id` int(10) unsigned NOT NULL,
  `is_blocked` tinyint(1) unsigned NOT NULL DEFAULT '0',
  `display_name` varchar(24) NOT NULL,
  `last_know_score` decimal(13,2) NOT NULL DEFAULT '0.00',
  `joined_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `keepalive_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  UNIQUE KEY `PK_livesession_viewers` (`livesession_id`,`user_id`),
  KEY `FK_ls_vi_uid_idx` (`user_id`),
  CONSTRAINT `FK_ls_vi_ls` FOREIGN KEY (`livesession_id`) REFERENCES `live_sessions` (`id`),
  CONSTRAINT `FK_ls_vi_uid` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.models
DROP TABLE IF EXISTS `models`;
CREATE TABLE IF NOT EXISTS `models` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(10) unsigned DEFAULT NULL,
  `name` varchar(20) NOT NULL,
  `description` varchar(191) NOT NULL,
  `website` varchar(191) DEFAULT NULL,
  `facebook` varchar(191) DEFAULT NULL,
  `twitter` varchar(191) DEFAULT NULL,
  `instagram` varchar(191) DEFAULT NULL,
  `snapchat` varchar(191) DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `shortname_UNIQUE` (`name`),
  UNIQUE KEY `user_id` (`user_id`),
  KEY `Fk_models_userid_idx` (`user_id`),
  CONSTRAINT `Fk_models_userid` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.model_products
DROP TABLE IF EXISTS `model_products`;
CREATE TABLE IF NOT EXISTS `model_products` (
  `model_id` int(10) unsigned DEFAULT NULL,
  `product_id` int(10) unsigned DEFAULT NULL,
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  UNIQUE KEY `model_id_product_id` (`model_id`,`product_id`),
  KEY `FK_model_products_product_id` (`product_id`),
  CONSTRAINT `FK_model_products_model_id` FOREIGN KEY (`model_id`) REFERENCES `models` (`id`),
  CONSTRAINT `FK_model_products_product_id` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.products
DROP TABLE IF EXISTS `products`;
CREATE TABLE IF NOT EXISTS `products` (
  `id` int(10) unsigned NOT NULL,
  `shortname` varchar(24) NOT NULL,
  `descr` varchar(191) NOT NULL,
  `active` tinyint(4) NOT NULL DEFAULT '1',
  `created_at` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `created_by` varchar(60) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `shortname_UNIQUE` (`shortname`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.purchases
DROP TABLE IF EXISTS `purchases`;
CREATE TABLE IF NOT EXISTS `purchases` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `wallet_id` int(10) unsigned NOT NULL,
  `amount` decimal(13,2) unsigned NOT NULL DEFAULT '0.00',
  `payment_trans_id` varchar(60) DEFAULT NULL,
  `payment_processor` varchar(60) DEFAULT NULL,
  `note` varchar(191) DEFAULT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `FK_purchases_wallet_idx` (`wallet_id`),
  CONSTRAINT `FK_purchases_wallet_id` FOREIGN KEY (`wallet_id`) REFERENCES `wallets` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=43 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.users
DROP TABLE IF EXISTS `users`;
CREATE TABLE IF NOT EXISTS `users` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `hash_key` char(32) NOT NULL COMMENT 'MD5',
  `src_domain` varchar(64) NOT NULL COMMENT 'callers member site TLD',
  `src_user_id` varchar(64) NOT NULL COMMENT 'callers member site user id',
  `login` varchar(60) NOT NULL,
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `src_user_ref_UNIQUE` (`hash_key`),
  UNIQUE KEY `src_domain_src_user_id` (`src_domain`,`src_user_id`)
) ENGINE=InnoDB AUTO_INCREMENT=34 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for tabel alpha69.wallets
DROP TABLE IF EXISTS `wallets`;
CREATE TABLE IF NOT EXISTS `wallets` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `user_id` int(10) unsigned NOT NULL,
  `product_id` int(10) unsigned DEFAULT NULL,
  `balance` decimal(13,2) unsigned DEFAULT '0.00',
  `total_tipped` decimal(13,2) unsigned DEFAULT '0.00',
  `total_purchased` decimal(13,2) unsigned DEFAULT '0.00',
  `created_at` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `UQ_wallets_user_product` (`user_id`,`product_id`),
  KEY `FK_wallets_user_id_idx` (`user_id`),
  KEY `FK_wallets_product_id_idx` (`product_id`),
  CONSTRAINT `FK_wallet_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`),
  CONSTRAINT `FK_wallets_product_id` FOREIGN KEY (`product_id`) REFERENCES `products` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
