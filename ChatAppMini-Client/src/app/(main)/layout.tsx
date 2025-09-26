import type { Metadata } from "next";
import { Toaster } from 'sonner';
import "../globals.css";
import { Navigation } from "@/components/home/navigation";
import axios from "axios";
import { cookies } from "next/headers";

export const metadata: Metadata = {
  title: "Chat App Mini",
  description: "Using NextJS v15, ASP.NET SignalR",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const userData = await axios.get("http://localhost:5189/api/users/@me", {
    headers: {
      Authorization: `Bearer ${(await cookies()).get("accessToken")?.value}`,
    },
    withCredentials: true
  });

  return (
    <>
      <header className="sticky top-0 z-50 backdrop-blur-sm">
        <div className="container mx-auto px-4 py-4">
          <Navigation userData={userData.data.data} />
        </div>
      </header>
      {children}
      <Toaster position="top-right" />
    </>
  );
}
