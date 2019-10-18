-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema school
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema school
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `school` ;
USE `school` ;

-- -----------------------------------------------------
-- Table `school`.`Districts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Districts` (
  `DistrictId` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`DistrictId`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`FiscalYears`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`FiscalYears` (
  `FiscalYearId` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(45) NULL DEFAULT NULL,
  `Start` INT(11) NOT NULL,
  `End` INT(11) NOT NULL,
  PRIMARY KEY (`FiscalYearId`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 6
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Budgets`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Budgets` (
  `BudgetId` INT(11) NOT NULL AUTO_INCREMENT,
  `DistrictId` INT(11) NOT NULL,
  `FiscalYearId` INT(11) NOT NULL,
  PRIMARY KEY (`BudgetId`),
  INDEX `FK_Districts_DistrictId_idx` (`DistrictId` ASC),
  INDEX `FK_Budget_FY_idx` (`FiscalYearId` ASC),
  CONSTRAINT `FK_Budget_District`
    FOREIGN KEY (`DistrictId`)
    REFERENCES `school`.`Districts` (`DistrictId`),
  CONSTRAINT `FK_Budget_FY`
    FOREIGN KEY (`FiscalYearId`)
    REFERENCES `school`.`FiscalYears` (`FiscalYearId`))
ENGINE = InnoDB
AUTO_INCREMENT = 31
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Revenues`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Revenues` (
  `RevenueId` INT(11) NOT NULL,
  `Level` CHAR(1) NOT NULL,
  `Description` VARCHAR(150) NOT NULL,
  PRIMARY KEY (`RevenueId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`BudgetRevenues`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`BudgetRevenues` (
  `BudgetRevenueId` INT(11) NOT NULL AUTO_INCREMENT,
  `BudgetId` INT(11) NOT NULL,
  `RevenueId` INT(11) NOT NULL,
  `Amount` INT(11) NOT NULL,
  PRIMARY KEY (`BudgetRevenueId`),
  UNIQUE INDEX `UQ_Budget_Revenue` (`BudgetId` ASC, `RevenueId` ASC),
  INDEX `FK_BudgetRevenue_Budget_idx` (`BudgetId` ASC),
  INDEX `FK_BudgetRevenue_Revenue_idx` (`RevenueId` ASC),
  CONSTRAINT `FK_BudgetRevenue_Budget`
    FOREIGN KEY (`BudgetId`)
    REFERENCES `school`.`Budgets` (`BudgetId`),
  CONSTRAINT `FK_BudgetRevenue_Revenue`
    FOREIGN KEY (`RevenueId`)
    REFERENCES `school`.`Revenues` (`RevenueId`))
ENGINE = InnoDB
AUTO_INCREMENT = 995
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Employees`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Employees` (
  `EmployeeId` INT(11) NOT NULL AUTO_INCREMENT,
  `FirstName` VARCHAR(50) NULL DEFAULT NULL,
  `LastName` VARCHAR(50) NULL DEFAULT NULL,
  `Salary` INT(10) UNSIGNED NULL DEFAULT NULL,
  `Degree` VARCHAR(45) NULL DEFAULT NULL,
  `Credits` INT(11) NULL DEFAULT NULL,
  `Step` INT(11) NULL DEFAULT NULL,
  `Location` VARCHAR(150) NULL DEFAULT NULL,
  `Position` VARCHAR(150) NULL DEFAULT NULL,
  PRIMARY KEY (`EmployeeId`))
ENGINE = InnoDB
AUTO_INCREMENT = 585
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Schools`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Schools` (
  `SchoolId` INT(11) NOT NULL AUTO_INCREMENT,
  `DistrictId` INT(11) NOT NULL,
  `Name` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`SchoolId`),
  UNIQUE INDEX `Name_UNIQUE` (`Name` ASC),
  INDEX `FK_Districts_DistrictId_idx` (`DistrictId` ASC),
  CONSTRAINT `FK_Districts_DistrictId`
    FOREIGN KEY (`DistrictId`)
    REFERENCES `school`.`Districts` (`DistrictId`))
ENGINE = InnoDB
AUTO_INCREMENT = 7
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Enrollments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Enrollments` (
  `EnrollmentId` INT(11) NOT NULL AUTO_INCREMENT,
  `FiscalYearId` INT(11) NOT NULL,
  `SchoolId` INT(11) NOT NULL,
  `GradeLevel` INT(11) NOT NULL,
  `Enrollment` INT(11) NOT NULL,
  PRIMARY KEY (`EnrollmentId`),
  UNIQUE INDEX `UQ_FY_School_Grade` (`FiscalYearId` ASC, `SchoolId` ASC, `GradeLevel` ASC),
  INDEX `FK_Schools_SchoolId_idx` (`SchoolId` ASC),
  INDEX `FK_FiscalYears_FiscalYearId_idx` (`FiscalYearId` ASC),
  CONSTRAINT `FK_FiscalYears_FiscalYearId`
    FOREIGN KEY (`FiscalYearId`)
    REFERENCES `school`.`FiscalYears` (`FiscalYearId`),
  CONSTRAINT `FK_Schools_SchoolId`
    FOREIGN KEY (`SchoolId`)
    REFERENCES `school`.`Schools` (`SchoolId`))
ENGINE = InnoDB
AUTO_INCREMENT = 113
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`ExpenditureCodes`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`ExpenditureCodes` (
  `Code` INT(11) NOT NULL,
  `Description` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Code`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`ExpendituresMidLevel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`ExpendituresMidLevel` (
  `MidLevelId` INT(11) NOT NULL,
  `Description` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`MidLevelId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`ExpendituresTopLevel`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`ExpendituresTopLevel` (
  `TopLevelId` INT(11) NOT NULL,
  `Description` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`TopLevelId`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`Expenditures`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`Expenditures` (
  `ExpenditureId` INT(11) NOT NULL AUTO_INCREMENT,
  `BudgetId` INT(11) NOT NULL,
  `TopLevelId` INT(11) NOT NULL,
  `MidLevelId` INT(11) NOT NULL,
  `CodeId` INT(11) NOT NULL,
  `Amount` INT(11) NOT NULL,
  PRIMARY KEY (`ExpenditureId`),
  UNIQUE INDEX `UQ_Exp_Budget_Top_Mid_Code_` (`BudgetId` ASC, `TopLevelId` ASC, `MidLevelId` ASC, `CodeId` ASC),
  INDEX `FK_Exp_Budget_idx` (`BudgetId` ASC),
  INDEX `FK_Exp_TopLevel_idx` (`TopLevelId` ASC),
  INDEX `FK_Exp_ExpMidLevel_idx` (`MidLevelId` ASC),
  INDEX `FK_Exp_ExpCode_idx` (`CodeId` ASC),
  CONSTRAINT `FK_Exp_Budget`
    FOREIGN KEY (`BudgetId`)
    REFERENCES `school`.`Budgets` (`BudgetId`),
  CONSTRAINT `FK_Exp_ExpCode`
    FOREIGN KEY (`CodeId`)
    REFERENCES `school`.`ExpenditureCodes` (`Code`),
  CONSTRAINT `FK_Exp_ExpMidLevel`
    FOREIGN KEY (`MidLevelId`)
    REFERENCES `school`.`ExpendituresMidLevel` (`MidLevelId`),
  CONSTRAINT `FK_Exp_ExpTopLevel`
    FOREIGN KEY (`TopLevelId`)
    REFERENCES `school`.`ExpendituresTopLevel` (`TopLevelId`))
ENGINE = InnoDB
AUTO_INCREMENT = 3308
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`HomeValues`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`HomeValues` (
  `HomeValueId` INT(11) NOT NULL AUTO_INCREMENT,
  `Value` INT(11) NULL DEFAULT NULL,
  PRIMARY KEY (`HomeValueId`))
ENGINE = InnoDB
AUTO_INCREMENT = 62
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `school`.`TotalDistrictEnrollments`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `school`.`TotalDistrictEnrollments` (
  `TotalDistrictEnrollmentId` INT(11) NOT NULL AUTO_INCREMENT,
  `DistrictId` INT(11) NOT NULL,
  `FiscalYearId` INT(11) NOT NULL,
  `Enrollment` INT(11) NOT NULL,
  PRIMARY KEY (`TotalDistrictEnrollmentId`),
  UNIQUE INDEX `UQ_District_FiscalYear` (`DistrictId` ASC, `FiscalYearId` ASC),
  INDEX `FK_TotalEnrollment_FiscalYear_idx` (`FiscalYearId` ASC),
  CONSTRAINT `FK_TotalEnrollment_District`
    FOREIGN KEY (`DistrictId`)
    REFERENCES `school`.`Districts` (`DistrictId`),
  CONSTRAINT `FK_TotalEnrollment_FiscalYear`
    FOREIGN KEY (`FiscalYearId`)
    REFERENCES `school`.`FiscalYears` (`FiscalYearId`))
ENGINE = InnoDB
AUTO_INCREMENT = 21
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
