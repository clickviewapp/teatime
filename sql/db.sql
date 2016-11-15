/*

 FOREIGN KEY naming scheme
	fk_{Table Name}_{Column Name}_{Referenced Table Name}_{Referenced Column Name}

  KEY naming scheme
    idx_{Table Name}_{Column Name}

*/


/*
# Create the database if it doesn't exist
*/
CREATE DATABASE IF NOT EXISTS teatime;

USE teatime;


#
# InventoryItems
#
CREATE TABLE `modules` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT ,
  `Name`     		VARCHAR(255)         	 NOT NULL,
  `Command`     	VARCHAR(255)         	 NOT NULL,
  `Description`     TEXT        		 DEFAULT NULL,
  
  `DateCreated`  	DATETIME	NOT NULL,
  `DateModified` 	DATETIME	DEFAULT NULL,
  `DateDeleted`  	DATETIME	DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  UNIQUE INDEX `uidx_modules_command` (`Command`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# InventoryItems
#
CREATE TABLE `inventory_items` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT ,
  `Name`     		VARCHAR(255)         	 NOT NULL,
  `Description`     TEXT        		 DEFAULT NULL,
  `ItemCode`     	VARCHAR(255)         DEFAULT NULL,
  
  `Quantity` 		INT 		DEFAULT(-1),
  
  `UnitPrice`       DECIMAL(10,2) DEFAULT 0.0,
  
  `ModuleId` BIGINT   		unsigned NOT NULL,
  
  `DateCreated`  	DATETIME	NOT NULL,
  `DateModified` 	DATETIME	DEFAULT NULL,
  `DateDeleted`  	DATETIME	DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  CONSTRAINT `fk_inventory_items_moduleid_modules_id`
    FOREIGN KEY (`ModuleId`)
    REFERENCES `modules` (`Id`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# InventoryItems
#
CREATE TABLE `inventory_item_options` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT ,
  `Name`     		VARCHAR(255)    NOT NULL,
  `Description`     TEXT        	DEFAULT NULL,
  
  `UnitPrice`       DECIMAL(10,2)   DEFAULT 0.0,
  `VALUE`     		VARCHAR(255)    DEFAULT NULL,
  
  `InventoryItemId` BIGINT   		unsigned NOT NULL,
  `DateCreated`  	DATETIME		NOT NULL,
  `DateModified` 	DATETIME		DEFAULT NULL,
  `DateDeleted`  	DATETIME		DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  CONSTRAINT `fk_inventory_item_options_inventoryitemid_inventory_items_id`
    FOREIGN KEY (`InventoryItemId`)
    REFERENCES `inventory_items` (`Id`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


#
# Users
#
CREATE TABLE `users` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT,
  `Name`     		VARCHAR(255)         	 DEFAULT NULL,
  `Surname`     	VARCHAR(255)         	 DEFAULT NULL,
  `Username`     	VARCHAR(255)         	 DEFAULT NULL,
  `Email`     		VARCHAR(255)         	 DEFAULT NULL,
  `UserId`     		VARCHAR(255)         	 DEFAULT NULL,
  
  `DateCreated`  	DATETIME	NOT NULL,
  `DateModified` 	DATETIME	DEFAULT NULL,
  `DateDeleted`  	DATETIME	DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  UNIQUE INDEX `uidx_users_userId` (`UserId`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;


#
# Users
#
CREATE TABLE `runs` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT,
  `Name`     		VARCHAR(255)         	 DEFAULT NULL,
  `Ended`			bit			NOT NULL DEFAULT 0,
  
  `UserId` 			BIGINT   		unsigned NOT NULL,
  `ModuleId` 		BIGINT   		unsigned NOT NULL,
  `ChannelId`     	VARCHAR(255)         	 NOT NULL,
  
  
  `DateCreated`  	DATETIME	NOT NULL,
  `DateModified` 	DATETIME	DEFAULT NULL,
  `DateDeleted`  	DATETIME	DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  CONSTRAINT `fk_runs_userid_users_id`
    FOREIGN KEY (`UserId`)
    REFERENCES `users` (`Id`),
	
  CONSTRAINT `fk_runs_moduleid_modules_id`
    FOREIGN KEY (`ModuleId`)
    REFERENCES `modules` (`Id`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

#
# orders
#
CREATE TABLE `orders` (
  `Id`     			BIGINT   		unsigned NOT NULL AUTO_INCREMENT,
  `Text`     		VARCHAR(255)    DEFAULT NULL,
  
  `UserId` 			BIGINT   		unsigned NOT NULL,
  `RunId` 		BIGINT   		unsigned NOT NULL,
  
  
  `DateCreated`  	DATETIME	NOT NULL,
  `DateModified` 	DATETIME	DEFAULT NULL,
  `DateDeleted`  	DATETIME	DEFAULT NULL,
  
  PRIMARY KEY (`id`),
  
  UNIQUE `unq_orders_user_run` (`RunId`, `UserId`),
  
  CONSTRAINT `fk_orders_userid_users_id`
    FOREIGN KEY (`UserId`)
    REFERENCES `users` (`Id`),
	
  CONSTRAINT `fk_orders_runid_runs_id`
    FOREIGN KEY (`RunId`)
    REFERENCES `runs` (`Id`)
  
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
