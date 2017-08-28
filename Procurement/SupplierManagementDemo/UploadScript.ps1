"NPM build started: ";
npm run build

"Copying Build started";
Copy-Item dist\build.js -Destination "C:\Users\f.jakab\KEEN MATE\Procurement - SupplierManagement";
Copy-Item dist\build.js.map -Destination "C:\Users\f.jakab\KEEN MATE\Procurement - SupplierManagement";
Copy-Item dist\build.js -Destination "C:\Users\f.jakab\Desktop\localTesting";
Copy-Item dist\build.js.map -Destination "C:\Users\f.jakab\Desktop\localTesting";
"Copying finished";