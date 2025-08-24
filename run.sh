#!/bin/bash

echo "🚀 Starting SA Banking App..."
echo "📱 Web Frontend: http://localhost:5000"
echo "🔧 API Documentation: http://localhost:5000/swagger"
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 8.0 or later."
    exit 1
fi

# Check .NET version
DOTNET_VERSION=$(dotnet --version)
echo "✅ .NET Version: $DOTNET_VERSION"

# Restore packages
echo "📦 Restoring packages..."
dotnet restore

# Build the project
echo "🔨 Building project..."
dotnet build

# Run the application
echo "🏃 Running application..."
echo ""
echo "Press Ctrl+C to stop the application"
echo ""

dotnet run 