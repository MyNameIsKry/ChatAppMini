import "@/app/globals.css"

export default async function AuthLayout({
    children
  }: Readonly<{
    children: React.ReactNode;
  }>) {
  
    return (
      <div>
          {children}
      </div>
    );
  }