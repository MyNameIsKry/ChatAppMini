"use client"

import { MessageCircle } from 'lucide-react';
import Link from 'next/link';
import React from 'react'
import { Button } from '../ui/button';
import { useAuthStore } from '@/store/auth';

export const Navigation = () => {
    const { logout, isAuthenticated, user } = useAuthStore((state) => state);
    
  return (
    <nav className="flex items-center justify-between">
        <div className="flex items-center space-x-2">
            <MessageCircle className="h-8 w-8 text-blue-600" />
            <span className="text-2xl font-bold text-gray-900">ChatMini</span>
        </div>
        <div className="space-x-4">
            { isAuthenticated ? (
                <Button variant="ghost" onClick={logout} className="cursor-pointer">Đăng xuất</Button>
            ) : (
                <>
                    <Link href="/login">
                        <Button variant="outline" className="cursor-pointer">Đăng nhập</Button>
                    </Link>
                    <Link href="/register">
                        <Button className="cursor-pointer">Đăng ký</Button>
                    </Link>
                </>
            )}
        </div>
    </nav>
  )
}
