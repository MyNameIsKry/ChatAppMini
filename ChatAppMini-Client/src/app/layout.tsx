import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Chat App Mini",
  description: "Using NextJS v15, ASP.NET SignalR",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body className="font-sans antialiased">
        {children}
      </body>
    </html>
  );
}
