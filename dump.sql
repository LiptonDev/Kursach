/*
 Navicat Premium Data Transfer

 Source Server         : localhost
 Source Server Type    : MySQL
 Source Server Version : 50525
 Source Host           : localhost:3306
 Source Schema         : kursach

 Target Server Type    : MySQL
 Target Server Version : 50525
 File Encoding         : 65001

 Date: 26/02/2020 17:53:29
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for groups
-- ----------------------------
DROP TABLE IF EXISTS `groups`;
CREATE TABLE `groups`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Название группы',
  `curatorId` int(11) NOT NULL COMMENT 'Куратор',
  `specialty` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Специальность',
  `start` date NOT NULL COMMENT 'Начало обучения',
  `end` date NOT NULL COMMENT 'Конец обучения',
  `isBudget` tinyint(1) NOT NULL COMMENT 'Бюджетная группа (0 - нет, 1 - да)',
  `division` int(11) NOT NULL COMMENT 'Подразделение (0 - 2)',
  `spoNpo` int(11) NOT NULL COMMENT '0 - СПО; 1 - НПО; 2 -ОВЗ',
  `isIntramural` tinyint(4) NOT NULL DEFAULT 1 COMMENT '0 - Зачное, 1 - очное',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `curatorId`(`curatorId`) USING BTREE,
  CONSTRAINT `groups_ibfk_1` FOREIGN KEY (`curatorId`) REFERENCES `staff` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE = InnoDB AUTO_INCREMENT = 98 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for signinlogs
-- ----------------------------
DROP TABLE IF EXISTS `signinlogs`;
CREATE TABLE `signinlogs`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `userId` int(11) NOT NULL COMMENT 'Пользователь',
  `date` datetime NOT NULL COMMENT 'Дата входа',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `userId`(`userId`) USING BTREE,
  CONSTRAINT `signinlogs_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE = InnoDB AUTO_INCREMENT = 267 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for staff
-- ----------------------------
DROP TABLE IF EXISTS `staff`;
CREATE TABLE `staff`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `firstName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Имя',
  `middleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Отчество',
  `lastName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Фамилия',
  `position` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Должность',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for students
-- ----------------------------
DROP TABLE IF EXISTS `students`;
CREATE TABLE `students`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `firstName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Имя',
  `middleName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Отчество',
  `lastName` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Фамилия',
  `poPkNumber` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '№ по п/к',
  `birthdate` datetime NOT NULL COMMENT 'Дата рождения',
  `decreeOfEnrollment` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Приказ о зачислении',
  `notice` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT 'Примечание',
  `groupId` int(11) NOT NULL COMMENT 'ИД группы',
  `expelled` tinyint(1) NOT NULL COMMENT 'Отчислен (0 - нет, 1 - да)',
  `onSabbatical` tinyint(1) NOT NULL COMMENT 'В академ. отпуске (0 - нет, 1 - да)',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `groupId`(`groupId`) USING BTREE,
  CONSTRAINT `students_ibfk_1` FOREIGN KEY (`groupId`) REFERENCES `groups` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE = InnoDB AUTO_INCREMENT = 4178 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

-- ----------------------------
-- Table structure for users
-- ----------------------------
DROP TABLE IF EXISTS `users`;
CREATE TABLE `users`  (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `login` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Логин',
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Имя пользователя',
  `password` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT 'Пароль',
  `mode` int(11) NOT NULL DEFAULT 1 COMMENT '0 - админ, 1 - чтение, 2 - чтение и запись',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `id`(`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 5 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Compact;

SET FOREIGN_KEY_CHECKS = 1;
