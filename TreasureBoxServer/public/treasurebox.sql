-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- 主机： 127.0.0.1
-- 生成日期： 2024-02-17 03:55:06
-- 服务器版本： 10.4.11-MariaDB
-- PHP 版本： 7.4.3

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- 数据库： `treasurebox`
--

-- --------------------------------------------------------

--
-- 表的结构 `account`
--

CREATE TABLE `account` (
  `uid` int(11) NOT NULL,
  `acctType` int(11) NOT NULL,
  `name` varchar(64) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `nickname` varchar(64) NOT NULL,
  `icon` int(11) NOT NULL DEFAULT 0,
  `money` int(11) NOT NULL DEFAULT 0,
  `diamond` int(11) NOT NULL DEFAULT 0,
  `lv` int(11) NOT NULL DEFAULT 1,
  `exp` int(11) NOT NULL DEFAULT 0,
  `lastLoginTime` datetime NOT NULL DEFAULT current_timestamp(),
  `vp` int(11) NOT NULL DEFAULT 0,
  `vpTime` datetime NOT NULL DEFAULT current_timestamp(),
  `vipExp` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `authentication`
--

CREATE TABLE `authentication` (
  `uid` int(11) NOT NULL,
  `loginName` varchar(50) NOT NULL,
  `password` varchar(32) NOT NULL,
  `token` varchar(32) NOT NULL,
  `tokenExpireTime` datetime NOT NULL DEFAULT current_timestamp(),
  `lastLoginTime` datetime NOT NULL DEFAULT current_timestamp(),
  `createTime` datetime NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `file`
--

CREATE TABLE `file` (
  `fileName` varchar(50) NOT NULL,
  `owner` int(11) NOT NULL,
  `data` mediumblob NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `item`
--

CREATE TABLE `item` (
  `uid` int(11) NOT NULL,
  `itemId` int(11) NOT NULL,
  `acctId` int(11) NOT NULL,
  `count` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `logaccount`
--

CREATE TABLE `logaccount` (
  `uid` int(11) NOT NULL,
  `acctId` int(11) NOT NULL,
  `logType` int(11) NOT NULL,
  `ip` varchar(16) NOT NULL,
  `createTime` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `logitem`
--

CREATE TABLE `logitem` (
  `uid` int(11) NOT NULL,
  `acctId` int(11) NOT NULL,
  `logType` int(11) NOT NULL,
  `changeCount` int(11) NOT NULL,
  `finalCount` int(11) NOT NULL,
  `createTime` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `logmail`
--

CREATE TABLE `logmail` (
  `uid` int(11) NOT NULL,
  `acctId` int(11) NOT NULL,
  `logType` int(11) NOT NULL,
  `title` text NOT NULL,
  `content` text NOT NULL,
  `appendix` text NOT NULL,
  `createTime` datetime NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `mail`
--

CREATE TABLE `mail` (
  `uid` int(11) NOT NULL,
  `acctId` int(11) NOT NULL,
  `senderId` int(11) NOT NULL,
  `senderName` text NOT NULL,
  `title` text NOT NULL,
  `content` text NOT NULL,
  `appendix` text NOT NULL,
  `createTime` datetime NOT NULL,
  `wasRead` tinyint(4) NOT NULL,
  `received` tinyint(4) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `message`
--

CREATE TABLE `message` (
  `uid` bigint(20) NOT NULL,
  `title` text NOT NULL,
  `question` text NOT NULL,
  `nickname` text NOT NULL,
  `ip` text NOT NULL,
  `hideIp` tinyint(4) NOT NULL,
  `contact` text NOT NULL,
  `createTime` timestamp NULL DEFAULT NULL,
  `answer` text NOT NULL,
  `answerTime` timestamp NULL DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- 表的结构 `signin`
--

CREATE TABLE `signin` (
  `acctId` int(11) NOT NULL,
  `month` int(11) NOT NULL,
  `signInList` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- 转储表的索引
--

--
-- 表的索引 `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`uid`),
  ADD UNIQUE KEY `index_name` (`name`,`acctType`);

--
-- 表的索引 `authentication`
--
ALTER TABLE `authentication`
  ADD PRIMARY KEY (`uid`),
  ADD UNIQUE KEY `authentication` (`loginName`);

--
-- 表的索引 `file`
--
ALTER TABLE `file`
  ADD PRIMARY KEY (`fileName`);

--
-- 表的索引 `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`uid`),
  ADD KEY `index_acctId` (`acctId`);

--
-- 表的索引 `logaccount`
--
ALTER TABLE `logaccount`
  ADD PRIMARY KEY (`uid`),
  ADD KEY `acctId` (`acctId`);

--
-- 表的索引 `logitem`
--
ALTER TABLE `logitem`
  ADD PRIMARY KEY (`uid`),
  ADD KEY `index_acctId` (`acctId`);

--
-- 表的索引 `logmail`
--
ALTER TABLE `logmail`
  ADD PRIMARY KEY (`uid`),
  ADD KEY `index_acctId` (`acctId`);

--
-- 表的索引 `mail`
--
ALTER TABLE `mail`
  ADD PRIMARY KEY (`uid`),
  ADD KEY `index_acctId` (`acctId`);

--
-- 表的索引 `message`
--
ALTER TABLE `message`
  ADD PRIMARY KEY (`uid`);

--
-- 表的索引 `signin`
--
ALTER TABLE `signin`
  ADD PRIMARY KEY (`acctId`);

--
-- 在导出的表使用AUTO_INCREMENT
--

--
-- 使用表AUTO_INCREMENT `account`
--
ALTER TABLE `account`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `authentication`
--
ALTER TABLE `authentication`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `item`
--
ALTER TABLE `item`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `logaccount`
--
ALTER TABLE `logaccount`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `logitem`
--
ALTER TABLE `logitem`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `logmail`
--
ALTER TABLE `logmail`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `mail`
--
ALTER TABLE `mail`
  MODIFY `uid` int(11) NOT NULL AUTO_INCREMENT;

--
-- 使用表AUTO_INCREMENT `message`
--
ALTER TABLE `message`
  MODIFY `uid` bigint(20) NOT NULL AUTO_INCREMENT;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
