# IceCream-Shop-Sales-Reports

## 📊 Sales Reports Dashboard (.NET 7 MVC)

This project is a simple yet insightful dashboard built using **ASP.NET Core MVC (.NET 7)** that visualizes sales data using **Chart.js**. It displays four distinct reports generated from a static dataset.

## 🚀 Features

- 🔹 **Report 1: Total Sales**
  - Displays the total revenue as a single value card at the top.
 
    ### Dashboard View
![Dashboard](wwwroot/assets/Screenshot 2025-05-29 012351.png)


- 🔹 **Report 2: Month-wise Sales Totals**
  - Visualized using a bar chart.
  - X-axis: Month (`Jan`, `Feb`...), Y-axis: Total sales.

- 🔹 **Report 3: Most Popular Item Each Month**
  - Grouped bar chart showing min, average, and max orders of the most popular item.
  - Hover reveals the item name for each month.

- 🔹 **Report 4: Revenue by Item Each Month**
  - Pie chart displaying top 3 items by revenue and one “Others” slice.
  - Dropdown lets you filter the chart by month.
  - Hovering over slices shows revenue values.

## 🧭 Navigation

- A **navbar** is available at the top with links to:
  - **Home**
  - **Dashboard**

- Click on **Dashboard** to access the overview screen showing links to all four reports.
- Click on a report title to view its detailed visualization.

## 📁 Dataset

- The dataset (`data.txt`) is located in:  
  `wwwroot/data/data.txt`

- The file is already included and versioned appropriately.

⚠️ **Note:** Do not share or commit sensitive datasets if working with real business data.

## 💻 Tech Stack

- **Backend:** ASP.NET Core MVC (.NET 7)
- **Frontend:** Razor Views, Chart.js
- **Chart Library:** [Chart.js](https://www.chartjs.org/)

## 🛠️ How to Run

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/sales-reports-dashboard.git


