import "./globals.css"

export default async function RootLayout({
    children
  }: Readonly<{
    children: React.ReactNode;
  }>) {
  
    return (
      <html lang="en">
        <body className='font-sans antialiased h-full w-full bg-gray-100'>
            {children}
        </body>
      </html>
    );
  }