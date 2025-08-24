# 🚀 NexusBank Deployment Guide

## 📋 **Deployment Options**

### **1. 🐳 Docker Deployment (Recommended for Local/Testing)**

#### **Quick Start:**
```bash
# Build and run with Docker Compose
docker-compose up --build

# Access your app at:
# HTTP:  http://localhost:8080
# HTTPS: https://localhost:8443
```

#### **Manual Docker Commands:**
```bash
# Build the image
docker build -t nexusbank .

# Run the container
docker run -d -p 8080:80 -p 8443:443 --name nexusbank-app nexusbank
```

### **2. ☁️ Azure App Service (Production Recommended)**

#### **Prerequisites:**
- Azure account
- Azure CLI installed
- GitHub repository connected

#### **Steps:**
1. **Create Azure Web App:**
   ```bash
   az group create --name nexusbank-rg --location eastus
   az appservice plan create --name nexusbank-plan --resource-group nexusbank-rg --sku B1
   az webapp create --name nexusbank-app --resource-group nexusbank-rg --plan nexusbank-plan --runtime "DOTNETCORE|8.0"
   ```

2. **Configure GitHub Actions:**
   - Go to your GitHub repository
   - Add secret: `AZURE_WEBAPP_PUBLISH_PROFILE`
   - Get publish profile from Azure portal
   - Push to main branch triggers deployment

3. **Access your app:**
   ```
   https://nexusbank-app.azurewebsites.net
   ```

### **3. 🚀 Heroku Deployment**

#### **Prerequisites:**
- Heroku account
- Heroku CLI installed

#### **Steps:**
```bash
# Login to Heroku
heroku login

# Create Heroku app
heroku create nexusbank-app

# Set buildpack
heroku buildpacks:set jincod/dotnetcore

# Deploy
git push heroku main

# Open app
heroku open
```

### **4. 🌊 DigitalOcean App Platform**

1. Go to DigitalOcean App Platform
2. Connect your GitHub repository
3. Select .NET 8.0 runtime
4. Configure environment variables
5. Deploy

### **5. 🚂 Railway Deployment**

1. Go to Railway.app
2. Connect your GitHub repository
3. Select .NET 8.0
4. Deploy automatically

## 🔧 **Environment Configuration**

### **Production Environment Variables:**
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80;https://+:443
```

### **Database Configuration:**
- Current: JSON file-based storage
- Production: Consider migrating to SQL Server/Azure SQL

## 📊 **Monitoring & Health Checks**

### **Health Check Endpoint:**
```
GET /health
```

### **Application Insights (Azure):**
- Add to `CSharpBankingApp.csproj`:
```xml
<PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.21.0" />
```

## 🔒 **Security Considerations**

### **Production Security:**
1. **HTTPS Only:** Enable HTTPS redirection
2. **CORS Policy:** Restrict to specific domains
3. **Authentication:** Implement proper JWT tokens
4. **Rate Limiting:** Add API rate limiting
5. **Secrets Management:** Use Azure Key Vault or similar

### **Update Program.cs for Production:**
```csharp
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
```

## 🚀 **Quick Deploy Commands**

### **Local Docker:**
```bash
# Start
docker-compose up -d

# Stop
docker-compose down

# View logs
docker-compose logs -f
```

### **Azure CLI:**
```bash
# Deploy to Azure
az webapp deployment source config-zip --resource-group nexusbank-rg --name nexusbank-app --src ./publish.zip
```

### **Manual Publish:**
```bash
# Publish for deployment
dotnet publish -c Release -o ./publish

# The publish folder contains your deployable app
```

## 📱 **Post-Deployment**

### **Verify Deployment:**
1. Check application loads correctly
2. Test authentication flow
3. Verify API endpoints
4. Check static files (CSS/JS) load
5. Test responsive design on mobile

### **Performance Monitoring:**
- Response times
- Memory usage
- CPU utilization
- Database performance

## 🆘 **Troubleshooting**

### **Common Issues:**
1. **Port conflicts:** Change ports in docker-compose.yml
2. **Build failures:** Check .NET version compatibility
3. **Static files not loading:** Verify wwwroot folder structure
4. **Database errors:** Check bankdata.json permissions

### **Logs:**
```bash
# Docker logs
docker logs nexusbank-app

# Azure logs
az webapp log tail --name nexusbank-app --resource-group nexusbank-rg
```

## 🎯 **Next Steps**

1. **Choose deployment method** based on your needs
2. **Set up monitoring** and logging
3. **Configure CI/CD** pipeline
4. **Set up staging environment**
5. **Implement backup strategy**

---

**Happy Deploying! 🚀** 