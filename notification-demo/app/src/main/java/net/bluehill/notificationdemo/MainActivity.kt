package net.bluehill.notificationdemo

import android.Manifest
import android.annotation.SuppressLint
import android.app.NotificationChannel
import android.app.NotificationManager
import android.content.pm.PackageManager
import android.graphics.Color
import android.os.Bundle
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.Button
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat
import androidx.core.content.ContextCompat
import net.bluehill.notificationdemo.ui.theme.NotificationDemoTheme

class MainActivity : ComponentActivity() {
    private val requestPermissionLauncher = registerForActivityResult(
        ActivityResultContracts.RequestPermission()
    ) { isGranted: Boolean ->
        if (!isGranted) {
            Toast.makeText(this, "설정에서 알림 권한을 허용해주세요.", Toast.LENGTH_SHORT).show()
            finishAffinity()
        }
    }

    @SuppressLint("MissingPermission", "UnusedMaterial3ScaffoldPaddingParameter")
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {
            val a = this
            val nmc = NotificationManagerCompat.from(a)

            NotificationDemoTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) {
                    Column(
                        horizontalAlignment = Alignment.CenterHorizontally,
                        verticalArrangement = Arrangement.Center,
                        modifier = Modifier.fillMaxSize()
                    ) {
                        Button({
                            val nb = NotificationCompat.Builder(a, "DemoTest")
                                .setSmallIcon(android.R.drawable.ic_dialog_info)
                                .setColor(Color.GREEN)
                                .setContentTitle("테스트 알림")
                                .setContentText("테스트 알림입니다.")
                                .setShortCriticalText("테스트")
                                .setPriority(NotificationCompat.PRIORITY_DEFAULT)
                                .setOngoing(true)
                                .setRequestPromotedOngoing(true)

                            nmc.notify(0, nb.build())
                        }) {
                            Text("알림 울리기")
                        }
                        Button({
                            nmc.cancel(0)
                        }) {
                            Text("알림 지우기")
                        }
                    }
                }
            }
        }
        askNotificationPermission()
        createNotificationChannel()
    }

    private fun askNotificationPermission() {
        when {
            ContextCompat.checkSelfPermission(
                this,
                Manifest.permission.POST_NOTIFICATIONS
            ) == PackageManager.PERMISSION_GRANTED -> {
            }

            shouldShowRequestPermissionRationale(Manifest.permission.POST_NOTIFICATIONS) -> {
                requestPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
            }

            else -> {
                requestPermissionLauncher.launch(Manifest.permission.POST_NOTIFICATIONS)
            }
        }
    }

    private fun createNotificationChannel() {
        val channel =
            NotificationChannel("DemoTest", "테스트", NotificationManager.IMPORTANCE_DEFAULT).apply {
                description = "테스트 카테고리"
            }

        getSystemService(NotificationManager::class.java).createNotificationChannel(channel)
    }
}
