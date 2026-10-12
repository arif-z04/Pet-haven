#!/usr/bin/env bash
# ============================================================================
# setup_database.sh
# Bash automation script to initialize MySQL database and load all scripts
# ============================================================================

set -e

DB_NAME="pet_ecommerce_db"
DB_USER="${MYSQL_USER:-root}"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=========================================================="
echo "Initializing $DB_NAME using user: $DB_USER"
echo "=========================================================="

# Check if MySQL/MariaDB client is installed
if ! command -v mysql &> /dev/null && ! command -v mariadb &> /dev/null; then
    echo "Error: Neither mysql nor mariadb client found in PATH."
    exit 1
fi

MYSQL_CMD="mysql"
if command -v mariadb &> /dev/null; then
    MYSQL_CMD="mariadb"
fi

echo "Using client: $MYSQL_CMD"
echo "Please enter MySQL password for user '$DB_USER' when prompted:"

cd "$SCRIPT_DIR"

$MYSQL_CMD -u "$DB_USER" -p < run_all.sql

echo "=========================================================="
echo "Successfully completed database deployment for $DB_NAME!"
echo "=========================================================="
