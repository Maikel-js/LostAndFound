#!/bin/bash
# Script to fix PostgreSQL authentication on Fedora
# Run as: sudo bash fix_postgres.sh

PG_HBA="/var/lib/pgsql/data/pg_hba.conf"

if [ ! -f "$PG_HBA" ]; then
    echo "Error: $PG_HBA not found."
    exit 1
fi

echo "Updating $PG_HBA..."
# Backup
cp "$PG_HBA" "${PG_HBA}.bak"

# Replace ident/peer with md5 for local and host
# We target the common Fedora default lines
sed -i '/^local/s/peer/md5/g' "$PG_HBA"
sed -i '/^local/s/ident/md5/g' "$PG_HBA"
sed -i '/^host/s/ident/md5/g' "$PG_HBA"

# Ensure host lines for md5 exist for 127.0.0.1
if ! grep -q "127.0.0.1/32" "$PG_HBA" | grep -q "md5"; then
    echo "host    all             all             127.0.0.1/32            md5" >> "$PG_HBA"
fi

echo "Restarting PostgreSQL..."
systemctl restart postgresql

echo "Setting password for user 'postgres' to 'postgres'..."
# Use peer auth via sudo -u postgres to set the password
sudo -u postgres psql -c "ALTER USER postgres WITH PASSWORD 'postgres';"

echo "Done! Please try running the API again."
